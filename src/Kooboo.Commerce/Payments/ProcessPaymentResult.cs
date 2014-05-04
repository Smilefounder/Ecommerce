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

        public static ProcessPaymentResult Success(string thirdPartyTransactionId = null)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Success,
                ThirdPartyTransactionId = thirdPartyTransactionId
            };
        }

        public static ProcessPaymentResult Failed(string errorMessage, string thirdPartyTransactionId = null)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Failed,
                ThirdPartyTransactionId = thirdPartyTransactionId,
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

        public static ProcessPaymentResult Reserved(string thirdPartyTransactionId)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Reserved,
                ThirdPartyTransactionId = thirdPartyTransactionId
            };
        }

        public static ProcessPaymentResult Pending(string redirectUrl, string thirdPartyTransactionId = null)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Pending,
                RedirectUrl = redirectUrl,
                ThirdPartyTransactionId = thirdPartyTransactionId
            };
        }
    }
}
