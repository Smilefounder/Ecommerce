using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class ProcessPaymentResult
    {
        public PaymentStatus PaymentStatus { get; set; }

        public string ErrorMessage { get; set; }

        public string PaymentTransactionId { get; set; }

        public string RedirectUrl { get; set; }

        public static ProcessPaymentResult Pending(string paymentTransactionId, string redirectUrl)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Pending,
                PaymentTransactionId = paymentTransactionId,
                RedirectUrl = redirectUrl
            };
        }

        public static ProcessPaymentResult Paid(string paymentTransactionId)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Success,
                PaymentTransactionId = paymentTransactionId
            };
        }

        public static ProcessPaymentResult Failed(string paymentTransactionId, string errorMessage)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Failed,
                PaymentTransactionId = paymentTransactionId,
                ErrorMessage = errorMessage
            };
        }
    }
}
