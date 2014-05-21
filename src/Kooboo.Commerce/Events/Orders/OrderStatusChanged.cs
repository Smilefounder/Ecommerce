using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    [Serializable]
    public class OrderStatusChanged : DomainEvent, IOrderEvent
    {
        [ConditionParameter]
        public int OrderId { get; set; }

        [ConditionParameter]
        public OrderStatus OldStatus { get; set; }

        [ConditionParameter]
        public OrderStatus NewStatus { get; set; }

        public OrderStatusChanged() { }

        public OrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus)
        {
            OrderId = order.Id;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
