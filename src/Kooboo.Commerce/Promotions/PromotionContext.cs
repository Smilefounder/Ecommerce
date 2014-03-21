using Kooboo.Commerce.Customers;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    /// <summary>
    /// Represents the context of a promotion.
    /// </summary>
    public class PromotionContext
    {
        public IList<ShoppingCartItem> Items { get; set; }

        public Customer Customer { get; set; }
    }
}
