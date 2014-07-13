using Kooboo.Commerce.Events;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Web.Framework.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    public class GetOrderActions : Event
    {
        [Reference(typeof(Order))]
        public int OrderId { get; private set; }

        public IList<string> ActionNames { get; private set; }

        public GetOrderActions(int orderId)
        {
            OrderId = orderId;
            ActionNames = new List<string>();
        }
    }
}
