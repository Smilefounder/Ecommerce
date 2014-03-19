using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Payments
{
    public class PaymentStatusChanged : IEvent
    {
        public Payment Payment { get; private set; }

        public PaymentStatus OldStatus { get; private set; }

        public PaymentStatus NewStatus { get; private set; }

        public PaymentStatusChanged(Payment payment, PaymentStatus oldStatus, PaymentStatus newStatus)
        {
            Payment = payment;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
