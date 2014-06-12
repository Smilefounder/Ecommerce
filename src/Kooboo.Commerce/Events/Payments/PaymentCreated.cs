using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Payments
{
    [Event(Order = 100)]
    public class PaymentCreated : BusinessEvent, IPaymentEvent
    {
        [Param]
        public int PaymentId { get; set; }

        public PaymentCreated() { }

        public PaymentCreated(Payment payment)
        {
            PaymentId = payment.Id;
        }
    }
}
