using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments
{
    public class PaymentProcessResult
    {
        public PaymentStatus PaymentStatus { get; set; }

        public string Message { get; set; }

        public string TransactionId { get; set; }

        public string RedirectUrl { get; set; }

        public static PaymentProcessResult Success(string transactionId = null)
        {
            return new PaymentProcessResult
            {
                PaymentStatus = PaymentStatus.Success,
                TransactionId = transactionId
            };
        }

        public static PaymentProcessResult Failed(string errorMessage, string transactionId = null)
        {
            return new PaymentProcessResult
            {
                PaymentStatus = PaymentStatus.Failed,
                TransactionId = transactionId,
                Message = errorMessage
            };
        }

        public static PaymentProcessResult Cancelled()
        {
            return new PaymentProcessResult
            {
                PaymentStatus = PaymentStatus.Cancelled
            };
        }

        public static PaymentProcessResult Pending(string redirectUrl, string transactionId = null)
        {
            return new PaymentProcessResult
            {
                PaymentStatus = PaymentStatus.Pending,
                RedirectUrl = redirectUrl,
                TransactionId = transactionId
            };
        }
    }
}
