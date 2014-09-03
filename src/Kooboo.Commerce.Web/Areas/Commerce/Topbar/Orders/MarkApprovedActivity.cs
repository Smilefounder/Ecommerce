using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Web.Areas.Commerce.Topbar.Orders.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Topbar.Orders
{
    public class MarkApprovedActivity : Activity<ApproveOrder>
    {
        public override string Name
        {
            get
            {
                return "ApproveOrder";
            }
        }

        private OrderService _service;

        public MarkApprovedActivity(OrderService service)
        {
            _service = service;
        }

        protected override void Execute(ApproveOrder @event, ActivityContext context)
        {
            var order = _service.GetById(@event.OrderId);
            order.ProcessingStatus = "Approved";
        }
    }
}