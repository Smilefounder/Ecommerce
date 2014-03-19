using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class PaymentMethodReference
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public PaymentMethodType Type { get; set; }

        public PaymentMethodReference() { }

        public PaymentMethodReference(PaymentMethod method)
        {
            Id = method.Id;
            DisplayName = method.DisplayName;
            Type = method.Type;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public class Payment : AggregateRoot
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethodReference PaymentMethod { get; set; }

        public TransactionType TransactionType { get; set; }

        public PaymentStatus Status { get; set; }

        public string ThirdPartyTransactionId { get; set; }

        /// <summary>
        /// The type of the object this payment is applied to.
        /// </summary>
        public string PaymentTargetType { get; set; }

        /// <summary>
        /// The key of the object this payment is applied to.
        /// </summary>
        public string PaymentTargetId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public Payment()
        {
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void Create()
        {
            Event.Apply(new PaymentCreated(this));
        }

        public void HandlePaymentResult(ProcessPaymentResult result)
        {
            if (Status == PaymentStatus.Success)
            {
                return;
            }

            var oldStatus = Status;

            if (oldStatus != result.PaymentStatus)
            {
                Status = result.PaymentStatus;
                Event.Apply(new PaymentStatusChanged(this, oldStatus, Status));
            }
        }
    }

    public static class PaymentTargetTypes
    {
        public static readonly string Order = "Order";
    }

    public enum TransactionType
    {
        Payment = 0,
        Refund = 1
    }
}
