using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class PromotionMatch
    {
        public Promotion Promotion { get; private set; }

        public IList<PricingItem> ConditionMatchedItems { get; private set; }

        public PromotionMatch(Promotion promotion, IEnumerable<PricingItem> conditionMatchedItems)
        {
            Promotion = promotion;
            ConditionMatchedItems = new List<PricingItem>(conditionMatchedItems);
        }
    }
}
