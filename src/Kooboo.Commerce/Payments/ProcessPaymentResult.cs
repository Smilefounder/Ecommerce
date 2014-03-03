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

        public string ErrorMessage { get; set; }

        public string PaymentTransactionId { get; set; }

        public ActionResult NextAction { get; set; }

        public static ProcessPaymentResult Success(string paymentTransactionId = null)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Success,
                PaymentTransactionId = paymentTransactionId
            };
        }

        public static ProcessPaymentResult Failed(string errorMessage, string paymentTransactionId = null)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Failed,
                PaymentTransactionId = paymentTransactionId,
                ErrorMessage = errorMessage
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
                PaymentTransactionId = transactionId
            };
        }

        public static ProcessPaymentResult Pending(ActionResult nextAction, string paymentTransactionId = null)
        {
            return new ProcessPaymentResult
            {
                PaymentStatus = PaymentStatus.Pending,
                NextAction = nextAction,
                PaymentTransactionId = paymentTransactionId
            };
        }
    }
}
