using Kooboo.Commerce.Events;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Topbar.Orders.Events
{
    [ActivityEvent(Category = "Orders", Order = 800)]
    public class ApproveOrder : Event
    {
        [Reference(typeof(Order))]
        public int OrderId { get; set; }

        public ApproveOrder(int orderId)
        {
            OrderId = orderId;
        }
    }
}