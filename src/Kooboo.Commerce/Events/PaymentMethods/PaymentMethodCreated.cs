using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.PaymentMethods
{
    [Event(Order = 100)]
    public class PaymentMethodCreated : DomainEvent, IPaymentMethodEvent
    {
    }
}
