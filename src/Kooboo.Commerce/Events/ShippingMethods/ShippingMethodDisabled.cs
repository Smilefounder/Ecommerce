using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShippingMethods
{
    [ActivityEvent(Order = 200)]
    public class ShippingMethodDisabled : Event, IShippingMethodEvent
    {
        [Reference(typeof(ShippingMethod))]
        public int ShippingMethodId { get; protected set; }

        protected ShippingMethodDisabled() { }

        public ShippingMethodDisabled(ShippingMethod method)
        {
            ShippingMethodId = method.Id;
        }
    }
}
