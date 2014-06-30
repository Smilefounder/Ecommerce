using System;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.Commerce.Payments
{
    public class Payment
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public int PaymentMethodId { get; set; }

        [Required, StringLength(100)]
        public string PaymentMethodName { get; set; }

        [Required, StringLength(100)]
        public string PaymentProcessorName { get; set; }

        public decimal PaymentMethodCost { get; set; }

        public PaymentStatus Status { get; set; }

        [StringLength(100)]
        public string ThirdPartyTransactionId { get; set; }

        public int OrderId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public Payment() { }

        public Payment(int orderId, decimal amount, PaymentMethod method, string description)
        {
            OrderId = orderId;
            Amount = amount;
            PaymentMethodId = method.Id;
            PaymentMethodName = method.Name;
            PaymentProcessorName = method.ProcessorName;
            PaymentMethodCost = method.GetPaymentMethodCost(amount);
            Description = description;
            CreatedAtUtc = DateTime.UtcNow;
        }
    }
}
