using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    public class OrderStatusChanged : IOrderEvent
    {
        public Order Order { get; protected set; }

        public OrderStatus OldStatus { get; protected set; }

        public OrderStatus NewStatus { get; protected set; }

        public OrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus)
        {
            Order = order;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
