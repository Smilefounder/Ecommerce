using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [Event(Order = 400)]
    public class CartItemRemoved : BusinessEvent, IShoppingCartEvent
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        public int ProductId { get; set; }

        public int ProductPriceId { get; set; }

        public int Quantity { get; set; }

        protected CartItemRemoved() { }

        public CartItemRemoved(ShoppingCart cart, ShoppingCartItem item)
        {
            CartId = cart.Id;
            ItemId = item.Id;
            ProductId = item.ProductPrice.ProductId;
            ProductPriceId = item.ProductPrice.Id;
            Quantity = item.Quantity;
        }
    }
}
