using Kooboo.Commerce.ComponentModel;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class Payment : INotifyObjectCreated
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethodReference PaymentMethod { get; set; }

        public decimal PaymentMethodCost { get; set; }

        public PaymentStatus Status { get; set; }

        public string ThirdPartyTransactionId { get; set; }

        public PaymentTarget PaymentTarget { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public Payment() { }

        public Payment(PaymentTarget target, decimal amount, PaymentMethod method, string description)
        {
            PaymentTarget = target;
            Amount = amount;
            PaymentMethod = new PaymentMethodReference(method);
            PaymentMethodCost = method.GetPaymentMethodCost(amount);
            Description = description;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void ChangeStatus(PaymentStatus newStatus)
        {
            var oldStatus = Status;
            if (oldStatus != newStatus)
            {
                Status = newStatus;
                Event.Raise(new PaymentStatusChanged(this, oldStatus, Status));
            }
        }

        void INotifyObjectCreated.NotifyCreated()
        {
            Event.Raise(new PaymentCreated(this));
        }
    }

    public static class PaymentTargetTypes
    {
        public static readonly string Order = "Order";
    }
}
