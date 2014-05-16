using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.DataSource;
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
            return null;
        }

        public bool HasAnyParameters()
        {
            return false;
        }
    }
}