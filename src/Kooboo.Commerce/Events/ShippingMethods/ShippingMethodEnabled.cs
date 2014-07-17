using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShippingMethods
{
    [ActivityEvent(Order = 100)]
    public class ShippingMethodEnabled : Event, IShippingMethodEvent
    {
        [Reference(typeof(ShippingMethod))]
        public int ShippingMethodId { get; protected set; }

        protected ShippingMethodEnabled() { }

        public ShippingMethodEnabled(ShippingMethod method)
        {
            ShippingMethodId = method.Id;
        }
    }
}
