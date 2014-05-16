using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Models;
using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

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
        public bool EnablePaging { get; set; }

        [DataMember]
        public string PageSize { get; set; }

        [DataMember]
        public string PageNumber { get; set; }

        public CommerceDataSource()
        {
            QueryFilters = new List<AddedQueryFilter>();
        }

        public object Execute(DataSourceContext dataSourceContext)
        {
            var api = EngineContext.Current.Resolve<ICommerceAPI>();
            api.InitCommerceInstance(GetInstanceName(dataSourceContext.Site), dataSourceContext.Site.Culture, null, dataSourceContext.Site.CustomFields);

            var descriptor = QueryDescriptors.GetDescriptor(QueryName);
            var query = EngineContext.Current.Resolve(descriptor.QueryType);

            // TODO: Exclude hal links for now
            CallMethod(query, "WithoutHalLinks");

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

                        var strValue = ParameterizedFieldValue.GetFieldValue(paramValue.ParameterValue, dataSourceContext.ValueProvider);
                        var value = Convert.ChangeType(strValue, param.Type);
                        parameters[i] = value;
                    }

                    CallMethod(query, filter.Method.Name, parameters);
                }
            }

            if (QueryType == QueryType.List)
            {
                if (EnablePaging)
                {
                    var strPageSize = ParameterizedFieldValue.GetFieldValue(PageSize, dataSourceContext.ValueProvider);
                    // TODO: How to handle default page size?
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

        private string GetInstanceName(Site site)
        {
            return site.CustomFields["CommerceInstance"];
        }

        private object CallMethod(object obj, string method, params object[] parameters)
        {
            var methodInfo = obj.GetType().GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            return methodInfo.Invoke(obj, parameters);
        }

        public bool HasAnyParameters()
        {
            return false;
        }
    }
}