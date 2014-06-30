using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class CheckPromotionConditionResult
    {
        public bool Success { get; set; }

        public IList<PriceCalculationItem> MatchedItems { get; private set; }

        public CheckPromotionConditionResult()
        {
            MatchedItems = new List<PriceCalculationItem>();
        }
    }
}
