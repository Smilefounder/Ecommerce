﻿using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [Event(Category = "Shopping Cart Events")]
    public interface IShoppingCartEvent : IDomainEvent
    {
        int CartId { get; }
    }
}
