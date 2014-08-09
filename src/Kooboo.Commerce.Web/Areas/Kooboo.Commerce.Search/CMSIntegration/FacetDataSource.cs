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
using Kooboo.Commerce.Search.Models;
using System.Reflection;
using Kooboo.Commerce.Utils;
using Kooboo.Commerce.Reflection;
using Kooboo.CMS.Sites.DataRule;

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

            var store = IndexStores.Get<ProductModel>(context.Instance, culture);
            var query = store.Query();

            if (Filters != null)
            {
                query = query.ApplyFilters(ParseFilters(context), culture);
            }

            return query.ToFacets(Facets);
        }

        private List<Filter> ParseFilters(CommerceDataSourceContext context)
        {
            if (Filters == null)
            {
                return new List<Filter>();
            }

            return Filters.Select(f => f.Parse(context.ValueProvider)).ToList();
        }

        public IDictionary<string, object> GetDefinitions(CommerceDataSourceContext context)
        {
            return new Dictionary<string, object>();
        }

        public IEnumerable<string> GetParameters()
        {
            return Enumerable.Empty<string>();
        }

        public bool IsEnumerable()
        {
            return true;
        }
    }
}
