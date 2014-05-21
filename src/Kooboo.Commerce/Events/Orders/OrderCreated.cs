using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    [Serializable]
    public class OrderCreated : DomainEvent, IOrderEvent
    {
        [ConditionParameter]
        public int OrderId { get; set; }

        public OrderCreated() { }

        public OrderCreated(Order order)
        {
            OrderId = order.Id;
        }
    }
}
