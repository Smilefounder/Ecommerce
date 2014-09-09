using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Carts;
using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Tracking
{
    class AddToCartTracker : IHandle<CartItemAdded>
    {
        public void Handle(CartItemAdded @event)
        {
            BehaviorReceivers.Receive(CommerceInstance.Current.Name,
                new Behavior {
                    ItemId = @event.ProductId.ToString(),
                    Type = BehaviorTypes.AddToCart,
                    UserId = new HttpContextWrapper(HttpContext.Current).EnsureVisitorUniqueId()
                });
        }
    }
}