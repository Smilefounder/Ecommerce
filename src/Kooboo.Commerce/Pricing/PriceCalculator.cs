using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Shipping;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Pricing
{
    [Dependency(typeof(PriceCalculator))]
    public class PriceCalculator
    {
        private IPromotionMatcher _promotionMatcher;
        private IPromotionPolicyFactory _promotionPolicyFactory;
        private IShippingRateProviderFactory _shippingRateProviderFactory;

        public PriceCalculator(
            IPromotionMatcher promotionMatcher,
            IPromotionPolicyFactory promotionPolicyFactory,
            IShippingRateProviderFactory shippingRateProviderFactory)
        {
            Require.NotNull(promotionMatcher, "promotionMatcher");
            Require.NotNull(promotionPolicyFactory, "promotionPolicyFactory");
            Require.NotNull(shippingRateProviderFactory, "shippingRateProviderFactory");

            _promotionMatcher = promotionMatcher;
            _promotionPolicyFactory = promotionPolicyFactory;
            _shippingRateProviderFactory = shippingRateProviderFactory;
        }

        public void Calculate(PricingContext context)
        {
            Require.NotNull(context, "context");

            foreach (var item in context.Items)
            {
                item.Total = item.Subtotal;
            }

            CalculatePaymentMethodFee(context);
            CalculateTax(context);
            CalculateShippingCost(context);

            context.Total = context.Items.Sum(x => x.Total)
                + context.Tax
                + context.PaymentMethodCost
                + context.ShippingCost;

            ApplyPromotions(context);
        }

        private void CalculatePaymentMethodFee(PricingContext context)
        {
            var paymentMethod = context.PaymentMethod;
            if (paymentMethod != null)
            {
                context.PaymentMethodCost = paymentMethod.GetPaymentMethodFee(context.Subtotal);
            }
        }

        private void CalculateShippingCost(PricingContext context)
        {
            var shippingMethod = context.ShippingMethod;
            if (shippingMethod != null)
            {
                var shippingRateProvider = _shippingRateProviderFactory.FindByName(shippingMethod.ShippingRateProviderName);
                var ctx = new ShippingCostCalculationContext
                {
                    Customer = context.Customer,
                    ShippingAddress = context.ShippingAddress,
                    Items = context.Items
                };

                context.ShippingCost = shippingRateProvider.CalculateShippingCost(shippingMethod, ctx);
            }
        }

        private void CalculateTax(PricingContext context)
        {
            // TODO: No tax implementation at the moment
        }

        private void ApplyPromotions(PricingContext context)
        {
            var matches = _promotionMatcher.MatchApplicablePromotions(context);

            foreach (var match in matches)
            {
                var policy = _promotionPolicyFactory.FindByName(match.Promotion.PromotionPolicyName);
                policy.Execute(new PromotionPolicyExecutionContext(match.Promotion, match.ConditionMatchedItems, context));
                context.AppliedPromotions.Add(match.Promotion);
            }
        }
    }

}
