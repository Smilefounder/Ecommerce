using Kooboo.Commerce.Events;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Topbar.Orders.Events
{
    public class ApproveOrder : IEvent
    {
        [Reference(typeof(Order))]
        public int OrderId { get; set; }

        public ApproveOrder(int orderId)
        {
            OrderId = orderId;
        }
    }
}