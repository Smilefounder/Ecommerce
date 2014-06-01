using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.PaymentMethods
{
    [Event(Order = 300)]
    public class PaymentMethodEnabled : DomainEvent, IPaymentMethodEvent
    {
    }
}
