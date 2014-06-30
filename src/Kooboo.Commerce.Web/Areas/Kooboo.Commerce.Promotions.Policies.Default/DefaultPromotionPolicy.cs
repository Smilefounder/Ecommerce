using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Promotions.Policies.Default
{
    public class DefaultPromotionPolicy : IPromotionPolicy, IHasCustomPromotionPolicyConfigEditor
    {
        public string Name
        {
            get
            {
                return "Discount Promotion";
            }
        }

        public Type ConfigModelType
        {
            get
            {
                return typeof(DefaultPromotionPolicyConfig);
            }
        }

        public void Execute(PromotionContext context)
        {
            var data = context.PolicyConfig as DefaultPromotionPolicyConfig;
            if (data == null)
            {
                return;
            }

            if (data.DiscountAppliedTo == DiscountAppliedTo.MatchedProducts)
            {
                foreach (var matchedItem in context.ConditionMatchedItems)
                {
                    matchedItem.Discount += ComputeDiscount(matchedItem.Subtotal, data);
                }
            }
            else if (data.DiscountAppliedTo == DiscountAppliedTo.OrderShipping)
            {
                context.PricingContext.ShippingCost -= ComputeDiscount(context.PricingContext.ShippingCost, data);
            }
            else if (data.DiscountAppliedTo == DiscountAppliedTo.OrderSubtotal)
            {
                context.PricingContext.Discount += ComputeDiscount(context.PricingContext.Subtotal, data);
            }
        }

        private decimal ComputeDiscount(decimal oldPrice, DefaultPromotionPolicyConfig policyData)
        {
            decimal discount = 0;

            if (policyData.DiscountMode == DiscountMode.ByAmount)
            {
                discount = policyData.DiscountAmount;
            }
            else
            {
                discount = oldPrice * ((decimal)policyData.DiscountPercent / 100);
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

        public string GetEditorVirtualPath(Promotion promotion)
        {
            return "~/Areas/" + Strings.AreaName + "/Views/Config.cshtml";
        }
    }
}