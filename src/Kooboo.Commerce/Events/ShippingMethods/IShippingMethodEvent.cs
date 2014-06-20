using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShippingMethods
{
    [Category("Shipping Methods", Order = 700)]
    public interface IShippingMethodEvent : IBusinessEvent
    {
        int ShippingMethodId { get; }
    }
}
