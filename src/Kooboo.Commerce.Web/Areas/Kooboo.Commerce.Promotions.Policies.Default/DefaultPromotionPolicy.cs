using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Promotions.Policies.Default
{
    [Dependency(typeof(IPromotionPolicy), Key = "Discount Promotion")]
    public class DefaultPromotionPolicy : IPromotionPolicy
    {
        public string Name
        {
            get
            {
                return "Discount Promotion";
            }
        }

        public void Execute(PromotionContext context)
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
                    matchedItem.Subtotal.AddDiscount(
                        ComputeDiscount(matchedItem.Subtotal.OriginalValue, data));
                }
            }
            else if (data.DiscountAppliedTo == DiscountAppliedTo.OrderShipping)
            {
                context.PricingContext.ShippingCost.AddDiscount(
                    ComputeDiscount(context.PricingContext.ShippingCost.OriginalValue, data));
            }
            else if (data.DiscountAppliedTo == DiscountAppliedTo.OrderSubtotal)
            {
                context.PricingContext.Subtotal.AddDiscount(
                    ComputeDiscount(context.PricingContext.Subtotal.OriginalValue, data));
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

        public PromotionPolicyEditor GetEditor()
        {
            return new PromotionPolicyEditor("~/Areas/" + Strings.AreaName + "/Views/Config.cshtml");
        }
    }
}