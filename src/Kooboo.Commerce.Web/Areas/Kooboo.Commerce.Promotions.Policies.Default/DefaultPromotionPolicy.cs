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
                    matchedItem.Discount += ComputeDiscount(matchedItem.Subtotal, data);
                }
            }
            else if (data.DiscountAppliedTo == DiscountAppliedTo.OrderShipping)
            {
                context.PriceCalculationContext.ShippingDiscount += ComputeDiscount(context.PriceCalculationContext.ShippingCost, data);
            }
            else if (data.DiscountAppliedTo == DiscountAppliedTo.OrderSubtotal)
            {
                context.PriceCalculationContext.DiscountExItemDiscounts += ComputeDiscount(context.PriceCalculationContext.Subtotal, data);
            }
        }

        private decimal ComputeDiscount(decimal oldPrice, DefaultPromotionPolicyData policyData)
        {
            decimal discount = 0;

            if (policyData.DiscountMode == PriceChangeMode.ByAmount)
            {
                discount = policyData.DiscountAmount;
            }
            else
            {
                discount = oldPrice * (decimal)policyData.DiscountPercent;
            }

            if (discount < 0)
            {
                discount = 0;
            }
            if (discount > oldPrice)
            {
                discount = oldPrice;
            }

            return discount;
        }
    }
}