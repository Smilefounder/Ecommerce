using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments
{
    public interface IPaymentGateway
    {
        string Name { get; }

        PaymentGatewayType PaymentGatewayType { get; }

        bool SupportBankSelection { get; }

        IEnumerable<BankInfo> GetSupportedBanks(PaymentMethod method);

        ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request);
    }

    public enum PaymentGatewayType
    {
        /// <summary>
        /// Customer is redirected to third-party payment gateway to complete the payment.
        /// </summary>
        RedirectedPayment = 0,

        /// <summary>
        /// Customer directly enter the credit card info in website.
        /// </summary>
        DirectCreditCardPayment = 1
    }

    public class CreditCardInfo
    {
        public string HolderName { get; set; }

        public string CardNumber { get; set; }

        public string Cvv2 { get; set; }

        /// <summary>
        /// Two character expired year.
        /// </summary>
        public int ExpiredYear { get; set; }

        /// <summary>
        /// Two character expired month.
        /// </summary>
        public int ExpiredMonth { get; set; }
    }
}
