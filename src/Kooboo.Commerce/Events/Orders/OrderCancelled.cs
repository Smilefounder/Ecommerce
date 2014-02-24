using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    public class OrderCancelled : IOrderEvent
    {
        public Order Order { get; private set; }

        public OrderCancelled(Order order)
        {
            Order = order;
        }
    }
}