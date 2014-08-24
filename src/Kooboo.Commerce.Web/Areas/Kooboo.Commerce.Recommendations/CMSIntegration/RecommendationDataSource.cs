using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Metadata;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.CMSIntegration;
using Kooboo.Commerce.CMSIntegration.DataSources;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic;
using Kooboo.Commerce.Recommendations.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.CMSIntegration
{
    public class RecommendationDataSource : GenericCommerceDataSource
    {
        public override string Name
        {
            get { return "Recommendations"; }
        }

        public override IEnumerable<FilterDescription> Filters
        {
            get
            {
                yield return new FilterDescription("ByProduct", new Int32ParameterDescription("ProductId", true));
            }
        }

        protected override object DoExecute(Commerce.CMSIntegration.DataSources.CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings)
        {
            // TODO: 未登录时应使用Cookie，但如何在用户访问产品页时生成Cookie呢？
            var userId = context.HttpContext.Session.SessionID;

            var top = settings.Top.GetValueOrDefault(4);
            var engine = GetRecommendationEngine(context, settings);

            var items = engine.Recommend(userId, top);
            var itemIds = items.Select(it => Convert.ToInt32(it.ItemId)).ToArray();

            var products = context.Site.Commerce()
                                  .Products.Query().ByIds(itemIds)
                                  .Include(p => p.Brand)
                                  .Include(p => p.Images)
                                  .Include(p => p.Variants)
                                  .ToList();

            var result = new List<Product>();

            foreach (var itemId in itemIds)
            {
                var product = products.Find(p => p.Id == itemId);
                if (product != null)
                {
                    result.Add(product);
                }
            }

            return result;
        }

        private IRecommendationEngine GetRecommendationEngine(CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings)
        {
            var filter = settings.Filters.Find(f => f.Name == "ByProduct");
            if (filter != null)
            {
                var productId = (int)filter.GetParameterValue("ProductId");
                var features = new[] { new Feature(productId.ToString()) };
                return new FeatureBasedRecommendationEngine(features, RelatedItemsReaders.GetReaders(context.Instance));
            }
            else
            {
                return RecommendationEngines.Get(context.Instance);
            }
        }
    }
}