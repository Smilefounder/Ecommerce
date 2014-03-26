using Kooboo.Commerce.Orders;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class PromotionPolicyExecutionContext
    {
        public Promotion Promotion { get; private set; }

        public PriceCalculationContext PriceCalculationContext { get; private set; }

        public IList<PricingItem> ConditionMatchedItems { get; private set; }

        public PromotionPolicyExecutionContext(
            Promotion promotion,
            IEnumerable<PricingItem> conditionMatchedItems,
            PriceCalculationContext priceCalculationContext)
        {
            Require.NotNull(promotion, "promotion");
            Require.NotNull(priceCalculationContext, "priceCalculationContext");

            Promotion = promotion;
            ConditionMatchedItems = (conditionMatchedItems ?? Enumerable.Empty<PricingItem>()).ToList();
            PriceCalculationContext = priceCalculationContext;
        }
    }
}
