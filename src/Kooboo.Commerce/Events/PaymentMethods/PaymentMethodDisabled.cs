using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.PaymentMethods
{
    [Event(Order = 400)]
    public class PaymentMethodDisabled : DomainEvent, IPaymentMethodEvent
    {
    }
}
