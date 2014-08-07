using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using Kooboo.Commerce.CMSIntegration;
using Kooboo.CMS.Sites.DataSource;
using Kooboo.Commerce.Search.Facets;
using System.Runtime.Serialization;
using Kooboo.Commerce.CMSIntegration.DataSources;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    [DataContract]
    [KnownType(typeof(FacetDataSource))]
    public class FacetDataSource : ICommerceDataSource
    {
        public string Name
        {
            get
            {
                return "Facets";
            }
        }

        public string EditorVirtualPath
        {
            get
            {
                return "~/Areas/" + Strings.AreaName + "/Views/DataSource/Facets.cshtml";
            }
        }

        [DataMember]
        public IList<Filter> Filters { get; set; }

        [DataMember]
        public IList<Facet> Facets { get; set; }

        [DataMember]
        public string SortField { get; set; }

        [DataMember]
        public SortDirection SortDirection { get; set; }

        public object Execute(CommerceDataSourceContext context)
        {
            return null;
        }

        public IDictionary<string, object> GetDefinitions(CommerceDataSourceContext context)
        {
            return new Dictionary<string, object>();
        }

        public IEnumerable<string> GetParameters()
        {
            yield break;
        }

        public bool IsEnumerable()
        {
            return true;
        }
    }
}
