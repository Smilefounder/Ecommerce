using System;
using System.Linq;
using System.Collections.Generic;
using Kooboo.Commerce.CMSIntegration;
using Kooboo.CMS.Sites.DataSource;
using Kooboo.Commerce.Search.Facets;
using System.Runtime.Serialization;
using Kooboo.Commerce.CMSIntegration.DataSources;
using Kooboo.Commerce.Products;
using System.Globalization;
using Lucene.Net.Search;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.CMS.Common.Runtime;

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

        public FacetDataSource()
        {
            Filters = new List<Filter>();
            Facets = new List<Facet>();
        }

        public object Execute(CommerceDataSourceContext context)
        {
            if (Facets == null || Facets.Count == 0)
            {
                return new List<FacetResult>();
            }

            var culture = CultureInfo.InvariantCulture;

            if (!String.IsNullOrEmpty(context.Site.Culture))
            {
                var languageStore = EngineContext.Current.Resolve<ILanguageStore>();
                if (languageStore.Exists(context.Site.Culture))
                {
                    culture = CultureInfo.GetCultureInfo(context.Site.Culture);
                }
            }

            var indexer = DocumentIndexers.GetLiveIndexer(context.Instance, typeof(Product), culture);
            return indexer.Facets(BuildQuery(), Facets);
        }

        // TODO: Complete it
        private Query BuildQuery()
        {
            return new MatchAllDocsQuery();
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
