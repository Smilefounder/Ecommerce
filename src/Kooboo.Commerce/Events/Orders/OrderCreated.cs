using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    public class OrderCreated : Event, IOrderEvent
    {
        public Order Order { get; set; }

        public OrderCreated(Order order)
        {
            Order = order;
        }
    }
}
