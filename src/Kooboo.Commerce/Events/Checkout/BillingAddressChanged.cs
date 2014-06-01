using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Checkout
{
    [Event(Order = 200)]
    public class BillingAddressChanged : DomainEvent, ICheckoutEvent
    {
    }
}
