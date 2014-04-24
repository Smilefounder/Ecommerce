using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    public class DeliveryBegan : Event, IOrderEvent
    {
        public Order Order { get; set; }

        public IList<OrderItem> DeliveredItems { get; set; }
    }
}
