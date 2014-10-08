using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShippingMethods
{
    public class ShippingMethodDisabled : IShippingMethodEvent
    {
        [Reference(typeof(ShippingMethod))]
        public int ShippingMethodId { get; set; }

        public ShippingMethodDisabled() { }

        public ShippingMethodDisabled(ShippingMethod method)
        {
            ShippingMethodId = method.Id;
        }
    }
}
