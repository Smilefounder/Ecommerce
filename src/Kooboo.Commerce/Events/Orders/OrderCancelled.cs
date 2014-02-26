using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    public class OrderCancelled : OrderStatusChanged
    {
        public OrderCancelled(Order order, OrderStatus oldStatus)
            : base(order, oldStatus, OrderStatus.Cancelled)
        {
        }
    }
}