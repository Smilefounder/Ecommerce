using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Promotions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing.Stages
{
    public class PromotionPricingStage : IPricingStage
    {
        private IPromotionService _promotionService;
        private IPromotionPolicyFactory _policyFactory;

        public PromotionPricingStage(
            IPromotionService promotionService,
            IPromotionPolicyFactory policyFactory)
        {
            Require.NotNull(promotionService, "promotionService");
            Require.NotNull(policyFactory, "policyFactory");

            _promotionService = promotionService;
            _policyFactory = policyFactory;
        }

        public void Execute(PricingContext context)
        {
            var matcher = new PromotionMatcher();
            var promotions = _promotionService.Query()
                                              .WhereAvailableNow()
                                              .ToList();

            var matches = matcher.MatchApplicablePromotions(context, promotions);

            foreach (var match in matches)
            {
                var policy = _policyFactory.Find(match.Promotion.PromotionPolicyName);
                if (policy == null)
                    throw new InvalidOperationException("Cannot load promotion policy with name '" + match.Promotion.PromotionPolicyName + "'. Ensure corresponding add-in has been installed.");

                policy.Execute(new PromotionContext(match.Promotion, match.ConditionMatchedItems, context));
                context.AppliedPromotions.Add(match.Promotion);
            }
        }
    }
}
