using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Web.Areas.Commerce.Common.Toolbar.Orders.Events;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.UI;
using Kooboo.Commerce.Web.Framework.UI.Toolbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Toolbar.Orders
{
    public class ApproveOrderCommand : OrderToolbarCommand
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
                return "Are you sure to approve?";
            }
        }

        public override Type ConfigType
        {
            get
            {
                return typeof(ApproveOrderCommandConfig);
            }
        }

        public override bool IsVisible(Order order, CommerceInstance instance)
        {
            return true;
        }

        public override ToolbarCommandResult Execute(Order order, object config, CommerceInstance instance)
        {
            Event.Raise(new ApproveOrder(order.Id));

            return ToolbarCommandResult.Succeeded().WithMessage("Order is approved.");
        }
    }
}