using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.Search.Api
{
    public class FacetsController : ApiController
    {
        private ILanguageStore _languageStore;

        // TODO: Fix web api dependency resolving and then remove this
        public FacetsController()
            : this(EngineContext.Current.Resolve<ILanguageStore>())
        {
        }

        public FacetsController(ILanguageStore languageStore)
        {
            _languageStore = languageStore;
        }

        public IEnumerable<FieldFacet> Get(string instance, string culture)
        {
            var cultureInfo = _languageStore.Exists(culture) ? CultureInfo.GetCultureInfo(culture) : CultureInfo.InvariantCulture;
            var indexer = DocumentIndexers.GetIndexer(instance, cultureInfo, typeof(Product));
            var fields = GetAllFields();
            return indexer.Facets(new MatchAllDocsQuery(), fields);
        }

        private string[] GetAllFields()
        {
            var fields = new HashSet<string> { };
            var instance = CommerceInstance.Current;

            foreach (var productType in instance.Database.GetRepository<ProductType>().Query())
            {
                foreach (var field in productType.VariantFieldDefinitions)
                {
                    fields.Add(field.Name);
                }
            }

            return fields.ToArray();
        }
    }
}
