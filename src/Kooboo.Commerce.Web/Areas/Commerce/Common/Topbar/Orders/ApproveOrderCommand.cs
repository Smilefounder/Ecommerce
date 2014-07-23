using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Web.Areas.Commerce.Common.Topbar.Orders.Events;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.UI;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Topbar.Orders
{
    public class ApproveOrderCommand : OrderTopbarCommand
    {
        public override string Name
        {
            get
            {
                return "ApproveOrder";
            }
        }

        public override string ButtonText
        {
            get
            {
                return "Approve";
            }
        }

        public override string ConfirmMessage
        {
            get
            {
                return "Are you sure to approve selected orders?";
            }
        }

        public override Type ConfigType
        {
            get
            {
                return typeof(ApproveOrderCommandConfig);
            }
        }

        public override bool CanExecute(Order order, CommerceInstance instance)
        {
            return order.Status == OrderStatus.Paid && order.ProcessingStatus != "Approved";
        }

        public override ActionResult Execute(IEnumerable<Order> orders, object config, CommerceInstance instance)
        {
            foreach (var order in orders)
            {
                Event.Raise(new ApproveOrder(order.Id));
            }

            return null;
        }
    }
}