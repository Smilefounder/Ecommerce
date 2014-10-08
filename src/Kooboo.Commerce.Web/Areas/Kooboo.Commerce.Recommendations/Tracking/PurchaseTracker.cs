using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Tracking
{
    class PurchaseTracker : IHandle<OrderStatusChanged>
    {
        public void Handle(OrderStatusChanged @event, EventContext context)
        {
            if (@event.NewStatus != OrderStatus.Paid)
            {
                return;
            }

            var behaviors = new List<Behavior>();

            var order = new OrderService(context.Instance).Find(@event.OrderId);
            foreach (var item in order.OrderItems)
            {
                behaviors.Add(new Behavior
                {
                    ItemId = item.ProductVariant.ProductId.ToString(),
                    UserId = order.CustomerId.ToString(),
                    Type = BehaviorTypes.Purchase
                });
            }

            BehaviorReceivers.Receive(context.Instance.Name, behaviors);
        }
    }
}