using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Payments
{
    public class PaymentCreated : Event
    {
        public Payment Payment { get; private set; }

        public PaymentCreated(Payment payment)
        {
            Payment = payment;
        }
    }
}
