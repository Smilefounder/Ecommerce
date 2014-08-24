using Kooboo.CMS.Sites.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    /// <summary>
    /// Represents as the bridge between ICommerceDataSource and CMS's IDataSource.
    /// It implements IDataSource and holds a reference to the selected ICommerceDataSource and delegate all execution logic to ICommerceDataSource.
    /// </summary>
    [DataContract]
    public class CommerceDataSourceAdapter : IDataSource
    {
        [DataMember]
        public ICommerceDataSource CommerceDataSource { get; set; }

        [DataMember]
        public string CommerceDataSourceType { get; set; }

        private void OnSerialized(StreamingContext context)
        {
            // Always ensure CommerceDataSource is not null, otherwise we are not able to deserialize it in data contract
            if (CommerceDataSource == null)
            {
                CommerceDataSource = TypeActivator.CreateInstance(Type.GetType(CommerceDataSourceType, true)) as ICommerceDataSource;
            }
        }

        public object Execute(DataSourceContext dataSourceContext)
        {
            return CommerceDataSource.Execute(CommerceDataSourceContext.CreateFrom(dataSourceContext, new HttpContextWrapper(HttpContext.Current)));
        }

        public IDictionary<string, object> GetDefinitions(DataSourceContext dataSourceContext)
        {
            return CommerceDataSource.GetDefinitions(CommerceDataSourceContext.CreateFrom(dataSourceContext, new HttpContextWrapper(HttpContext.Current)));
        }

        public IEnumerable<string> GetParameters()
        {
            return CommerceDataSource.GetParameters();
        }

        public bool IsEnumerable()
        {
            return CommerceDataSource.IsEnumerable();
        }
    }
}