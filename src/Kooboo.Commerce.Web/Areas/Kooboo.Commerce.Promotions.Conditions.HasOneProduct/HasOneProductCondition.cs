using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Promotions.Conditions.HasOneProduct
{
    [Description("At least one of specified products is added to cart")]
    [Dependency(typeof(IPromotionCondition), Key = "Kooboo.Commerce.Promotions.Conditions.HasOneProduct")]
    public class HasOneProductCondition : IPromotionCondition
    {
        public string Name
        {
            get
            {
                return Strings.ConditionName;
            }
        }

        public bool CanMatchProducts
        {
            get
            {
                return true;
            }
        }

        public string GetDescription(PromotionCondition requirement)
        {
            var settings = new HasOneProductConditionData();
            if (requirement != null && !String.IsNullOrEmpty(requirement.ConditionData))
            {
                settings = HasOneProductConditionData.Deserialize(requirement.ConditionData);
            }

            return "At least one of these products is added to cart: " + String.Join(", ", settings.ProductIds);
        }

        public ConditionCheckingResult Check(ConditionCheckingContext context)
        {
            if (String.IsNullOrEmpty(context.Condition.ConditionData))
            {
                return ConditionCheckingResult.Failed();
            }

            var ruleData = HasOneProductConditionData.Deserialize(context.Condition.ConditionData);

            if (String.IsNullOrEmpty(ruleData.ProductIds))
            {
                return ConditionCheckingResult.Failed();
            }

            var productIds = ruleData.ProductIds
                                     .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(x => Convert.ToInt32(x.Trim())).ToList();

            var matchedItems = context.PricingContext.Items.Where(it => productIds.Contains(it.Product.Id)).ToList();

            if (matchedItems.Count > 0)
            {
                return new ConditionCheckingResult(true)
                {
                    MatchedItems = matchedItems
                };
            }

            return ConditionCheckingResult.Failed();
        }
    }
}