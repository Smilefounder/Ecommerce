using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class PromotionContext
    {
        public Promotion Promotion { get; private set; }

        public object PolicyConfig { get; private set; }

        public PriceCalculationContext PricingContext { get; private set; }

        public IList<PriceCalculationItem> ConditionMatchedItems { get; private set; }

        public PromotionContext(
            Promotion promotion, object policyConfig, IEnumerable<PriceCalculationItem> conditionMatchedItems, PriceCalculationContext pricingContext)
        {
            Require.NotNull(promotion, "promotion");
            Require.NotNull(pricingContext, "pricingContext");

            Promotion = promotion;
            PolicyConfig = policyConfig;
            ConditionMatchedItems = (conditionMatchedItems ?? Enumerable.Empty<PriceCalculationItem>()).ToList();
            PricingContext = pricingContext;
        }
    }
}
