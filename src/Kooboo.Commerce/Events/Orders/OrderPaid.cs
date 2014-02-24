using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Orders;

namespace Kooboo.Commerce.Events.Orders
{
    public class OrderPaid : IOrderEvent
    {
        public Order Order { get; private set; }

        public OrderPaid(Order order)
        {
            Order = order;
        }
    }
}