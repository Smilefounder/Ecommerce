using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShippingMethods
{
    [Event(Order = 100)]
    public class ShippingMethodCreated : BusinessEvent, IShippingMethodEvent
    {
    }
}
