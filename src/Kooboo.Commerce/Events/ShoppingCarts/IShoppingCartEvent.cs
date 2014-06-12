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
    [Category("Shopping Carts", Order = 800)]
    public interface IShoppingCartEvent : IBusinessEvent
    {
        int CartId { get; }
    }
}
