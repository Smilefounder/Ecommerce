using Kooboo.Commerce.Orders;
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

        public IList<ShoppingCartItem> ConditionMatchedItems { get; private set; }

        public PromotionMatch(Promotion promotion, IEnumerable<ShoppingCartItem> conditionMatchedItems)
        {
            Promotion = promotion;
            ConditionMatchedItems = new List<ShoppingCartItem>(conditionMatchedItems);
        }
    }
}
