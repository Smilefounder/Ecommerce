using Kooboo.Commerce.ComponentModel;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class Payment
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethodInfo PaymentMethod { get; set; }

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
            PaymentMethod = new PaymentMethodInfo(method);
            PaymentMethodCost = method.GetPaymentMethodCost(amount);
            Description = description;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public static Payment CreateOrderPayment(int orderId, decimal amount, PaymentMethod paymentMethod, string description)
        {
            return new Payment(new PaymentTarget(orderId.ToString(), PaymentTargetTypes.Order), amount, paymentMethod, description);
        }
    }

    public static class PaymentTargetTypes
    {
        public static readonly string Order = "Order";
    }
}
