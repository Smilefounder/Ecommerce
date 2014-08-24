using Kooboo.Commerce.Api.Metadata;
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
                yield return new FilterDescription("ByProduct", new Int32ParameterDescription("ProductId", false));
            }
        }

        protected override object DoExecute(Commerce.CMSIntegration.DataSources.CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings)
        {
            // TODO: 未登录时应使用Cookie，但如何在用户访问产品页时生成Cookie呢？
            var userId = context.HttpContext.Session.SessionID;

            var top = settings.Top.GetValueOrDefault(4);
            var engine = GetRecommendationEngine(context, settings);

            var items = engine.Recommend(userId, top);

            return items;
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