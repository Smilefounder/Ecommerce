using Kooboo.Commerce.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Membership;
using System.Globalization;
using System.Reflection;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    public abstract class ApiBasedDataSource : GenericCommerceDataSource
    {
        protected ApiQueryDescriptor Descriptor { get; private set; }

        protected List<FilterDefinition> InternalFilters;
        protected HashSet<string> InternalSortableFields = new HashSet<string>();
        protected HashSet<string> InternalIncludablePaths;

        protected Type QueryType { get; private set; }

        protected Type ItemType { get; private set; }

        private string _name;

        public override string Name
        {
            get
            {
                return _name;
            }
        }

        public override IEnumerable<FilterDefinition> Filters
        {
            get
            {
                return InternalFilters;
            }
        }

        public override IEnumerable<string> SortableFields
        {
            get
            {
                return InternalSortableFields;
            }
        }

        public override IEnumerable<string> IncludablePaths
        {
            get
            {
                return InternalIncludablePaths;
            }
        }

        protected ApiBasedDataSource(string name, Type queryType, Type itemType)
        {
            _name = name;
            QueryType = queryType;
            ItemType = itemType;

            var descriptor = ApiQueryDescriptor.GetDescriptor(queryType);

            InternalFilters = descriptor.Filters.Select(f => f.ToFilterDefinition()).ToList();
            InternalIncludablePaths = new HashSet<string>(descriptor.IncludablePaths);

            Descriptor = descriptor;
        }

        protected abstract object GetQuery(ICommerceApi api);

        protected override object DoExecute(CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings)
        {
            var user = new HttpContextWrapper(HttpContext.Current).Membership().GetMembershipUser();
            var accountId = user == null ? null : user.UUID;

            var apiContext = new ApiContext(context.Site.GetCommerceInstanceName(), CultureInfo.GetCultureInfo(context.Site.Culture), null, accountId);
            var api = ApiService.Get(context.Site.ApiType(), apiContext);

            var query = GetQuery(api);

            if (settings.Filters != null && settings.Filters.Count > 0)
            {
                ApplyFilters(query, settings.Filters, context);
            }
            if (settings.Includes != null && settings.Includes.Count > 0)
            {
                ApplyIncludes(query, settings.Includes, context);
            }

            // Executing result
            if (settings.TakeOperation == Kooboo.Commerce.CMSIntegration.DataSources.TakeOperation.List)
            {
                if (settings.EnablePaging)
                {
                    var pageSize = settings.PageSize.GetValueOrDefault(10);
                    var pageNumber = settings.PageNumber.GetValueOrDefault(1);

                    return CallMethod(query, "Pagination", pageNumber - 1, pageSize);
                }

                return CallMethod(query, "ToArray");
            }
            else
            {
                return CallMethod(query, "FirstOrDefault");
            }
        }

        public override IDictionary<string, object> GetDefinitions(CommerceDataSourceContext context)
        {
            return DataSourceDefinitionHelper.GetDefinitions(ItemType);
        }

        protected virtual void ApplyFilters(object query, List<ParsedFilter> filters, CommerceDataSourceContext context)
        {
            foreach (var filter in filters)
            {
                var definition = Descriptor.Filters.FirstOrDefault(f => f.Name == filter.Name);
                if (definition != null)
                {
                    var parameters = new object[definition.Parameters.Count];
                    for (var i = 0; i < definition.Parameters.Count; i++)
                    {
                        var param = definition.Parameters[i];
                        var paramValue = filter.ParameterValues[param.Name];
                        parameters[i] = filter.ParameterValues[param.Name];
                    }

                    CallMethod(query, definition.Method.Name, parameters);
                }
            }
        }

        protected object CallMethod(object obj, string method, params object[] parameters)
        {
            var methodInfo = obj.GetType().GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (methodInfo == null)
                throw new InvalidOperationException("Method '" + method + "' was not found.");

            return methodInfo.Invoke(obj, parameters);
        }

        protected virtual void ApplyIncludes(object query, IEnumerable<string> includes, CommerceDataSourceContext context)
        {
            var methods = query.GetType()
                               .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                               .Where(m => m.Name == "Include");

            MethodInfo includeMethod = null;

            foreach (var method in methods)
            {
                var args = method.GetParameters();
                if (args.Length == 1 && args[0].ParameterType == typeof(string))
                {
                    includeMethod = method;
                    break;
                }
            }

            if (includeMethod == null)
                throw new InvalidOperationException("Cannot find Include(string path) method in query api.");

            foreach (var path in includes)
            {
                includeMethod.Invoke(query, new object[] { path });
            }
        }
    }
}