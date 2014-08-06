using Kooboo.CMS.Sites.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    [DataContract]
    public class CommerceDataSourceAdapter : IDataSource
    {
        [DataMember]
        public ICommerceDataSource DataSource { get; set; }

        public object Execute(DataSourceContext dataSourceContext)
        {
            return DataSource.Execute(new CommerceDataSourceContext());
        }

        public IDictionary<string, object> GetDefinitions(DataSourceContext dataSourceContext)
        {
            return DataSource.GetDefinitions(new CommerceDataSourceContext());
        }

        public IEnumerable<string> GetParameters()
        {
            return DataSource.GetParameters();
        }

        public bool IsEnumerable()
        {
            return DataSource.IsEnumerable();
        }
    }
}