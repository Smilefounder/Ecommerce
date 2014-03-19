using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public class Payment
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
