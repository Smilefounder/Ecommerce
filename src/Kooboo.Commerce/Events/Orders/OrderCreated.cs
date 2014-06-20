using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    [Event(Order = 100, ShortName = "Created")]
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
