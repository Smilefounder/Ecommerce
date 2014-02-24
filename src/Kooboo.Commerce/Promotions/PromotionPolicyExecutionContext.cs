using Kooboo.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class PromotionPolicyExecutionContext
    {
        public Promotion Promotion { get; private set; }

        public ICollection<PricingItem> ConditionMatchedItems { get; private set; }

        public PricingContext PricingContext { get; private set; }

        public PromotionPolicyExecutionContext(
            Promotion promotion,
            IEnumerable<PricingItem> conditionMatchedItems,
            PricingContext pricingContext)
        {
            Require.NotNull(promotion, "promotion");
            Require.NotNull(pricingContext, "pricingContext");

            Promotion = promotion;
            ConditionMatchedItems = (conditionMatchedItems ?? Enumerable.Empty<PricingItem>()).ToList();
            PricingContext = pricingContext;
        }
    }
}
