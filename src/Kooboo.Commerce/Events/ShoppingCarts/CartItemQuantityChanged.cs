using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [Event(Order = 300)]
    public class CartItemQuantityChanged : DomainEvent, IShoppingCartEvent
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        public int OldQuantity { get; set; }

        public int NewQuantity { get; set; }

        public CartItemQuantityChanged() { }

        public CartItemQuantityChanged(ShoppingCart cart, ShoppingCartItem item, int oldQuantity)
        {
            CartId = cart.Id;
            ItemId = item.Id;
            OldQuantity = oldQuantity;
            NewQuantity = item.Quantity;
        }
    }
}
