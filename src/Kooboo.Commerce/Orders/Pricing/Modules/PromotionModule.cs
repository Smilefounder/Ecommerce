using Kooboo.Commerce.Events;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing.Modules
{
    public class PromotionModule : IPriceCalculationModule
    {
        private PromotionService _promotionService;
        private IPromotionPolicyProvider _policyFactory;
        private ConditionEvaluator _ruleEngine;

        public PromotionModule(
            PromotionService promotionService,
            IPromotionPolicyProvider policyFactory,
            ConditionEvaluator ruleEngine)
        {
            Require.NotNull(promotionService, "promotionService");
            Require.NotNull(policyFactory, "policyFactory");
            Require.NotNull(ruleEngine, "ruleEngine");

            _promotionService = promotionService;
            _policyFactory = policyFactory;
            _ruleEngine = ruleEngine;
        }

        public void Execute(PriceCalculationContext context)
        {
            var matcher = new PromotionMatcher(_ruleEngine);
            var promotions = _promotionService.Query().WhereAvailableNow().ToList();

            var matches = matcher.MatchApplicablePromotions(context, promotions);

            foreach (var match in matches)
            {
                var policy = _policyFactory.FindByName(match.Promotion.PromotionPolicyName);
                if (policy == null)
                    throw new InvalidOperationException("Cannot load promotion policy with name '" + match.Promotion.PromotionPolicyName + "'. Ensure corresponding add-in has been installed.");

                object config = null;
                if (policy.ConfigType != null)
                {
                    config = match.Promotion.LoadPolicyConfig(policy.ConfigType);
                }

                policy.Execute(new PromotionContext(match.Promotion, config, match.ConditionMatchedItems, context));
                context.AppliedPromotions.Add(match.Promotion);
            }
        }
    }
}
