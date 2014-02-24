using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [EventCategory("Shopping Cart Events")]
    public interface IShoppingCartEvent : IDomainEvent
    {
        ShoppingCart Cart { get; }
    }
}
