using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    [DataContract]
    public class AddedQueryFilter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<QueryFilterParameterValue> ParameterValues { get; set; }

        public AddedQueryFilter()
        {
            ParameterValues = new List<QueryFilterParameterValue>();
        }
    }

    [DataContract]
    public class QueryFilterParameterValue
    {
        [DataMember]
        public string ParameterName { get; set; }

        [DataMember]
        public string ParameterValue { get; set; }
    }

    public enum QueryType
    {
        List = 0,
        FirstOrDefault = 1
    }

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
            return null;
        }

        public bool HasAnyParameters()
        {
            return false;
        }
    }
}