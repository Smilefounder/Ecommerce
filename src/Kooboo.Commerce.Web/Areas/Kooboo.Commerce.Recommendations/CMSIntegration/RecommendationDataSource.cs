using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Metadata;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.CMSIntegration;
using Kooboo.Commerce.CMSIntegration.DataSources;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic;
using Kooboo.Commerce.Recommendations.Engine;
using Kooboo.Commerce.Recommendations.Engine.Behaviors;
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
            var userId = context.HttpContext.GetVisitorUniqueId();

            var top = settings.Top.GetValueOrDefault(4);
            ISet<string> toIgnoreItems;

            var engine = GetRecommendationEngine(context, settings, out toIgnoreItems);

            var items = engine.Recommend(userId, top, toIgnoreItems);
            var itemIds = items.Select(it => Convert.ToInt32(it.ItemId)).ToArray();

            var reasonItemIds = new List<int>();
            foreach (var item in items)
            {
                var reasonId = item.GetBestReasonItemId();
                if (reasonId > 0)
                {
                    reasonItemIds.Add(reasonId);
                }
            }

            var products = context.Site.Commerce()
                                  .Products.Query().ByIds(itemIds)
                                  .Include(p => p.Brand)
                                  .Include(p => p.Images)
                                  .Include(p => p.Variants)
                                  .ToList();

            var reasons = context.Site.Commerce()
                                 .Products.Query().ByIds(reasonItemIds.ToArray())
                                 .ToList();

            var result = new List<ProductRecommendation>();

            foreach (var item in items)
            {
                var productId = Convert.ToInt32(item.ItemId);
                var product = products.Find(p => p.Id == productId);
                if (product != null)
                {
                    var recommendation = new ProductRecommendation
                    {
                        Product = product,
                        Weight = item.Weight
                    };

                    var reasonId = item.GetBestReasonItemId();
                    if (reasonId > 0)
                    {
                        recommendation.Reason = reasons.Find(p => p.Id == reasonId);
                    }

                    result.Add(recommendation);
                }
            }

            return result;
        }

        private IRecommendationEngine GetRecommendationEngine(CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings, out ISet<string> toIgnoreItems)
        {
            var filter = settings.Filters.Find(f => f.Name == "ByProduct");
            if (filter != null)
            {
                var productId = (int)filter.GetParameterValue("ProductId");

                toIgnoreItems = new HashSet<string> { productId.ToString() };

                return new FeatureBasedRecommendationEngine(new[] { new Feature(productId.ToString()) }, RelatedItemsProviders.All(context.Instance));
            }
            else
            {
                toIgnoreItems = new HashSet<string>();

                foreach (var behaviorType in BehaviorTypes.All())
                {
                    var store = BehaviorStores.Get(context.Instance, behaviorType);
                    foreach (var itemId in store.GetItemsUserHadBehaviorsOn(context.HttpContext.EnsureVisitorUniqueId(), 10000))
                    {
                        toIgnoreItems.Add(itemId);
                    }
                }

                return RecommendationEngines.Get(context.Instance);
            }
        }
    }
}