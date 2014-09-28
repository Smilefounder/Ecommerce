using Kooboo.CMS.Sites.DataSource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    [DataContract]
    public abstract class CommerceDataSource : IDataSource
    {
        [JsonProperty]
        public abstract string Name { get; }

        public virtual string EditorVirtualPath
        {
            get
            {
                return null;
            }
        }

        object IDataSource.Execute(DataSourceContext dataSourceContext)
        {
            return Execute(CommerceDataSourceContext.CreateFrom(dataSourceContext, new HttpContextWrapper(HttpContext.Current)));
        }

        public abstract object Execute(CommerceDataSourceContext context);

        IDictionary<string, object> IDataSource.GetDefinitions(DataSourceContext dataSourceContext)
        {
            return GetDefinitions(CommerceDataSourceContext.CreateFrom(dataSourceContext, new HttpContextWrapper(HttpContext.Current)));
        }

        public abstract IDictionary<string, object> GetDefinitions(CommerceDataSourceContext context);

        public abstract IEnumerable<string> GetParameters();

        public abstract bool IsEnumerable();
    }
}