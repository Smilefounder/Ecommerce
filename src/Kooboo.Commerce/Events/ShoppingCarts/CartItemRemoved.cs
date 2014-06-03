using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [Event(Order = 400)]
    public class CartItemRemoved : DomainEvent, IShoppingCartEvent
    {
        public int CartId { get; set; }
    }
}
