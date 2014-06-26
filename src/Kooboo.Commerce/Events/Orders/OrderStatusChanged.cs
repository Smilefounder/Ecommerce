using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    public class OrderStatusChanged : Event, IOrderEvent
    {
        [Reference(typeof(Order))]
        public int OrderId { get; set; }

        [Param(Name = "Order.OldStatus")]
        public OrderStatus? OldStatus { get; set; }

        [Param(Name = "Order.NewStatus")]
        public OrderStatus NewStatus { get; set; }

        protected OrderStatusChanged() { }

        public OrderStatusChanged(Order order, OrderStatus? oldStatus, OrderStatus newStatus)
        {
            OrderId = order.Id;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
