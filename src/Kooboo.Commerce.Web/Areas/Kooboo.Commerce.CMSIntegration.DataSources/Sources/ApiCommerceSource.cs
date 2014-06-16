using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Models;
using Kooboo.Commerce.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public abstract class ApiCommerceSource : ICommerceSource
    {
        internal ApiQueryDescriptor Descriptor { get; private set; }

        protected List<SourceFilterDefinition> InternalFilters;
        protected HashSet<string> InternalSortableFields = new HashSet<string>();
        protected HashSet<string> InternalIncludablePaths;

        protected Type QueryType { get; private set; }

        public string Name { get; protected set; }

        public IEnumerable<SourceFilterDefinition> Filters
        {
            get
            {
                return InternalFilters;
            }
        }

        public IEnumerable<string> SortableFields
        {
            get
            {
                return InternalSortableFields;
            }
        }

        public IEnumerable<string> IncludablePaths
        {
            get
            {
                return InternalIncludablePaths;
            }
        }

        protected ApiCommerceSource(string name, Type queryType)
        {
            Name = name;
            QueryType = queryType;

            var descriptor = ApiQueryDescriptor.GetDescriptor(queryType);

            InternalFilters = descriptor.Filters.Select(f => f.ToFilterDefinition()).ToList();
            InternalIncludablePaths = new HashSet<string>(descriptor.IncludablePaths);

            Descriptor = descriptor;
        }

        public virtual object Execute(CommerceSourceContext context)
        {
            var dataSourceContext = context.DataSourceContext;
            var api = EngineContext.Current.Resolve<ICommerceAPI>();
            api.InitCommerceInstance(GetInstanceName(dataSourceContext.Site), dataSourceContext.Site.Culture, null, dataSourceContext.Site.CustomFields);

            var query = EngineContext.Current.Resolve(QueryType);

            // TODO: Exclude hal links for now
            CallMethod(query, "WithoutHalLinks");

            if (context.Filters != null && context.Filters.Count > 0)
            {
                ApplyFilters(query, context.Filters, dataSourceContext.ValueProvider);
            }
            if (context.Includes != null && context.Includes.Count > 0)
            {
                ApplyIncludes(query, context.Includes);
            }

            // Executing result
            if (context.TakeOperation == Kooboo.Commerce.CMSIntegration.DataSources.TakeOperation.List)
            {
                if (context.EnablePaging)
                {
                    var pageSize = context.PageSize.GetValueOrDefault(10);
                    var pageNumber = context.PageNumber.GetValueOrDefault(1);

                    return CallMethod(query, "Pagination", pageNumber - 1, pageSize);
                }

                return CallMethod(query, "ToArray");
            }
            else
            {
                return CallMethod(query, "FirstOrDefault");
            }
        }

        private void ApplyFilters(object query, IEnumerable<SourceFilter> filters, IValueProvider valueProvider)
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

        private string GetInstanceName(Site site)
        {
            return site.CustomFields["CommerceInstance"];
        }

        private object CallMethod(object obj, string method, params object[] parameters)
        {
            var methodInfo = obj.GetType().GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            return methodInfo.Invoke(obj, parameters);
        }

        private void ApplyIncludes(object query, IEnumerable<string> includes)
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