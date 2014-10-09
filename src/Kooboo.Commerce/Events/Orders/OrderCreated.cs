using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    public class OrderCreated : IOrderEvent
    {
        [Reference(typeof(Order))]
        public int OrderId { get; set; }

        protected OrderCreated() { }

        public OrderCreated(Order order)
        {
            OrderId = order.Id;
        }
    }
}
