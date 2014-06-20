using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    [Event(Order = 200, ShortName = "Payment Status Changed")]
    public class PaymentStatusChanged : BusinessEvent, IOrderEvent
    {
        [Param]
        public int OrderId { get; set; }

        [Param]
        public int PaymentId { get; set; }

        [Param]
        public decimal Amount { get; set; }

        [Param]
        public PaymentStatus? OldStatus { get; set; }

        [Param]
        public PaymentStatus NewStatus { get; set; }

        [Reference(typeof(PaymentMethod))]
        public int PaymentMethodId { get; set; }

        protected PaymentStatusChanged() { }

        public PaymentStatusChanged(Payment payment, PaymentStatus? oldStatus, PaymentStatus newStatus)
        {
            // TODO: Ugly?
            OrderId = Convert.ToInt32(payment.PaymentTarget.Id);

            PaymentId = payment.Id;
            Amount = payment.Amount;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            PaymentMethodId = payment.PaymentMethod.Id;
        }
    }
}
