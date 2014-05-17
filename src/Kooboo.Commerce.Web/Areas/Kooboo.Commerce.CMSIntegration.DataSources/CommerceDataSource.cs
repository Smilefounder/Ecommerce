using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Models;
using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    [DataContract(Name = "CommerceDataSource")]
    [KnownType(typeof(CommerceDataSource))]
    public class CommerceDataSource : IDataSource
    {
        [DataMember]
        public string QueryName { get; set; }

        [DataMember]
        public QueryType QueryType { get; set; }

        [DataMember]
        public List<AddedQueryFilter> QueryFilters { get; set; }

        [DataMember]
        public List<string> Includes { get; set; }

        [DataMember]
        public bool EnablePaging { get; set; }

        [DataMember]
        public string PageSize { get; set; }

        [DataMember]
        public string PageNumber { get; set; }

        public CommerceDataSource()
        {
            QueryFilters = new List<AddedQueryFilter>();
            Includes = new List<string>();
        }

        public object Execute(DataSourceContext dataSourceContext)
        {
            var api = EngineContext.Current.Resolve<ICommerceAPI>();
            api.InitCommerceInstance(GetInstanceName(dataSourceContext.Site), dataSourceContext.Site.Culture, null, dataSourceContext.Site.CustomFields);

            var descriptor = QueryDescriptors.GetDescriptor(QueryName);
            var query = EngineContext.Current.Resolve(descriptor.QueryType);

            // TODO: Exclude hal links for now
            CallMethod(query, "WithoutHalLinks");

            ApplyFilters(query, descriptor, dataSourceContext.ValueProvider);
            ApplyIncludes(query);

            // Executing result
            if (QueryType == QueryType.List)
            {
                if (EnablePaging)
                {
                    var strPageSize = ParameterizedFieldValue.GetFieldValue(PageSize, dataSourceContext.ValueProvider);
                    if (!String.IsNullOrWhiteSpace(strPageSize))
                    {
                        var pageSize = Convert.ToInt32(strPageSize);
                        var strPageNumber = ParameterizedFieldValue.GetFieldValue(PageNumber, dataSourceContext.ValueProvider);
                        if (String.IsNullOrWhiteSpace(strPageNumber))
                        {
                            strPageNumber = "1";
                        }

                        var pageNumber = Convert.ToInt32(strPageNumber);

                        return CallMethod(query, "Pagination", pageNumber - 1, pageSize);
                    }
                }

                return CallMethod(query, "ToArray");
            }
            else
            {
                return CallMethod(query, "FirstOrDefault");
            }
        }

        private void ApplyFilters(object query, QueryDescriptor descriptor, IValueProvider valueProvider)
        {
            foreach (var addedFilter in QueryFilters)
            {
                var filter = descriptor.Filters.FirstOrDefault(f => f.Name == addedFilter.Name);
                if (filter != null)
                {
                    var parameters = new object[filter.Parameters.Count];
                    for (var i = 0; i < filter.Parameters.Count; i++)
                    {
                        var param = filter.Parameters[i];
                        var paramValue = addedFilter.ParameterValues.Find(p => p.ParameterName == param.Name);
                        if (paramValue == null)
                            throw new InvalidOperationException("Invalid datasource settings. Required filter method parameter '" + param.Name + "'.");

                        var strValue = ParameterizedFieldValue.GetFieldValue(paramValue.ParameterValue, valueProvider);
                        var value = Convert.ChangeType(strValue, param.Type);
                        parameters[i] = value;
                    }

                    CallMethod(query, filter.Method.Name, parameters);
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

        private void ApplyIncludes(object query)
        {
            if (Includes == null || Includes.Count == 0)
            {
                return;
            }

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

            foreach (var path in Includes)
            {
                includeMethod.Invoke(query, new object[] { path });
            }
        }

        public bool HasAnyParameters()
        {
            foreach (var filter in QueryFilters)
            {
                if (filter.ParameterValues.Any(v => ParameterizedFieldValue.IsParameterizedField(v.ParameterValue)))
                {
                    return true;
                }
            }

            if (ParameterizedFieldValue.IsParameterizedField(PageSize))
            {
                return true;
            }

            if (ParameterizedFieldValue.IsParameterizedField(PageNumber))
            {
                return true;
            }

            return false;
        }
    }
}