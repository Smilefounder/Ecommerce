using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments
{
    public class ProcessPaymentResult
    {
        public PaymentStatus PaymentStatus { get; set; }

        public string Message { get; set; }

        public string ThirdPartyTransactionId { get; set; }

        public string RedirectUrl { get; set; }

        public static ProcessPaymentResult Success(string paymentTransactionId = null)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Success,
                ThirdPartyTransactionId = paymentTransactionId
            };
        }

        public static ProcessPaymentResult Failed(string errorMessage, string paymentTransactionId = null)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Failed,
                ThirdPartyTransactionId = paymentTransactionId,
                Message = errorMessage
            };
        }

        public static ProcessPaymentResult Cancelled()
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Cancelled
            };
        }

        public static ProcessPaymentResult Reserved(string transactionId)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Reserved,
                ThirdPartyTransactionId = transactionId
            };
        }

        public static ProcessPaymentResult Pending(string redirectUrl, string paymentTransactionId = null)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Pending,
                RedirectUrl = redirectUrl,
                ThirdPartyTransactionId = paymentTransactionId
            };
        }
    }
}
