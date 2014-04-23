using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    public class OrderStatusChanged : Event, IOrderEvent
    {
        public Order Order { get; private set; }

        public OrderStatus OldStatus { get; private set; }

        public OrderStatus NewStatus { get; private set; }

        public OrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus)
        {
            Order = order;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
