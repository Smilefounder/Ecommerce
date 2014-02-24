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

        public string AuthorizationTransactionId { get; set; }

        public string AuthorizationTransactionCode { get; set; }

        public string NormalPaymentTransactionId { get; set; }

        public string RedirectUrl { get; set; }

        public static ProcessPaymentResult Pending(string normalPaymentTransactionId, string redirectUrl)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Pending,
                NormalPaymentTransactionId = normalPaymentTransactionId,
                RedirectUrl = redirectUrl
            };
        }

        public static ProcessPaymentResult Paid(string externalTransactionId)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Success,
                NormalPaymentTransactionId = externalTransactionId
            };
        }

        public static ProcessPaymentResult Failed(string externalTransactionId, string errorMessage)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Failed,
                NormalPaymentTransactionId = externalTransactionId,
                ErrorMessage = errorMessage
            };
        }
    }
}
