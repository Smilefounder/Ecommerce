using Kooboo.Commerce.Data;
using Kooboo.Commerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders
{
    public class PriceCalculator
    {
        private IRepository<Promotion> _promotionRepository;
        private IPromotionPolicyFactory _promotionPolicyFactory;

        public PriceCalculator(
            IRepository<Promotion> promotionRepository,
            IPromotionPolicyFactory promotionPolicyFactory)
        {
            _promotionRepository = promotionRepository;
            _promotionPolicyFactory = promotionPolicyFactory;
        }

        public void Calculate(PriceCalculationContext context)
        {
            CalculateShippingCost(context);
            CalculatePaymentMethodCost(context);
            ApplyPromotions(context);
        }

        private void CalculateShippingCost(PriceCalculationContext context)
        {
            // TODO: Calculate shipping cost
        }

        private void CalculatePaymentMethodCost(PriceCalculationContext context)
        {
            if (context.PaymentMethod != null)
            {
                context.PaymentMethodCost = context.PaymentMethod.GetPaymentMethodCost(context.Subtotal);
            }
        }

        private void ApplyPromotions(PriceCalculationContext context)
        {
            var matcher = new PromotionMatcher();
            var promotions = _promotionRepository.Query().WhereAvailableNow().ToList();
            var matches = matcher.MatchApplicablePromotions(context, promotions);

            foreach (var match in matches)
            {
                var policy = _promotionPolicyFactory.FindByName(match.Promotion.PromotionPolicyName);
                policy.Execute(new PromotionPolicyExecutionContext(match.Promotion, match.ConditionMatchedItems));

                context.AppliedPromotions.Add(match.Promotion);
            }
        }
    }
}
