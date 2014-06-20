using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShippingMethods
{
    [Event(Order = 300, ShortName = "Enabled")]
    public class ShippingMethodEnabled : BusinessEvent, IShippingMethodEvent
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
