using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShippingMethods
{
    public interface IShippingMethodEvent : IEvent
    {
        int ShippingMethodId { get; }
    }
}
