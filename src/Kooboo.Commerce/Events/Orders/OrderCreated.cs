using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    // TODO: Not fired status changed?
    [Event(Order = 100)]
    public class OrderCreated : OrderStatusChanged
    {
        protected OrderCreated() { }

        public OrderCreated(Order order)
            : base(order, null, OrderStatus.Created)
        {
            OrderId = order.Id;
        }
    }
}
