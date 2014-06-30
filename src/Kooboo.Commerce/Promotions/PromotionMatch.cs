using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class PromotionMatch
    {
        public Promotion Promotion { get; private set; }

        public IList<PriceCalculationItem> ConditionMatchedItems { get; private set; }

        public PromotionMatch(Promotion promotion, IEnumerable<PriceCalculationItem> conditionMatchedItems)
        {
            Promotion = promotion;
            ConditionMatchedItems = new List<PriceCalculationItem>(conditionMatchedItems);
        }
    }
}
