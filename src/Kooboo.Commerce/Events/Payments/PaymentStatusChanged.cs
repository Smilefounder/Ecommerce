using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Payments
{
    [Serializable]
    public class PaymentStatusChanged : DomainEvent, IPaymentEvent
    {
        [ConditionParameter]
        public int PaymentId { get; set; }

        [ConditionParameter]
        public PaymentStatus OldStatus { get; set; }

        [ConditionParameter]
        public PaymentStatus NewStatus { get; set; }

        public PaymentStatusChanged() { }

        public PaymentStatusChanged(Payment payment, PaymentStatus oldStatus, PaymentStatus newStatus)
        {
            PaymentId = payment.Id;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
