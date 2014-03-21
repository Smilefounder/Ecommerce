using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Promotions.Policies.Default
{
    [Dependency(typeof(IPromotionPolicy), Key = "Kooboo.Commerce.Promotions.Policies.Default")]
    public class DefaultPromotionPolicy : IPromotionPolicy
    {
        public string Name
        {
            get
            {
                return Strings.PolicyName;
            }
        }

        public string DisplayName
        {
            get
            {
                return "Discount Promotion";
            }
        }

        public void Execute(PromotionPolicyExecutionContext context)
        {
            var promotion = context.Promotion;

            if (String.IsNullOrEmpty(promotion.PromotionPolicyData))
            {
                return;
            }

            var data = DefaultPromotionPolicyData.Deserialize(promotion.PromotionPolicyData);

            if (data.DiscountAppliedTo == DiscountAppliedTo.MatchedProducts)
            {
                foreach (var matchedItem in context.ConditionMatchedItems)
                {
                    // TODO: Fix needed
                    //matchedItem.ApplyDiscount(ComputeDiscount(matchedItem.Subtotal, data));
                }
            }
            else if (data.DiscountAppliedTo == DiscountAppliedTo.OrderShipping)
            {
                //context.PricingContext.ApplyShippingDiscount(ComputeDiscount(context.PricingContext.ShippingCost, data));
            }
            else if (data.DiscountAppliedTo == DiscountAppliedTo.OrderSubtotal)
            {
                //context.PricingContext.ApplyShippingDiscount(ComputeDiscount(context.PricingContext.Subtotal, data));
            }
        }

        private decimal ComputeDiscount(decimal oldPrice, DefaultPromotionPolicyData policyData)
        {
            decimal newPrice = 0;

            if (policyData.DiscountMode == PriceChangeMode.ByAmount)
            {
                newPrice = oldPrice - policyData.DiscountAmount;
            }
            else
            {
                newPrice = oldPrice * (decimal)policyData.DiscountPercent;
            }

            if (newPrice < 0)
            {
                newPrice = 0;
            }

            return newPrice;
        }
    }
}