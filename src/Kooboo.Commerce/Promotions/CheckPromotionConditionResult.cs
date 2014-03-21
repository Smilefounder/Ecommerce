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

        public IList<ShoppingCartItem> MatchedItems { get; set; }
    }
}
