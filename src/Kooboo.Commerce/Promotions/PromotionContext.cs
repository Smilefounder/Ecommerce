using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class PromotionContext
    {
        public Promotion Promotion { get; private set; }

        public PricingContext PricingContext { get; private set; }

        public IList<PricingItem> ConditionMatchedItems { get; private set; }

        public PromotionContext(
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
