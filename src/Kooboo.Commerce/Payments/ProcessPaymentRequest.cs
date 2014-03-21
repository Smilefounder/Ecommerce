using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Orders;
using System.Web;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Payments
{
    public class ProcessPaymentRequest
    {
        public Payment Payment { get; set; }

        public decimal Amount
        {
            get
            {
                return Payment.Amount;
            }
        }

        public string CurrencyCode { get; set; }

        public CreditCardInfo CreditCardInfo { get; set; }

        public BankAccountInfo BankAccountInfo { get; set; }

        public string ReturnUrl { get; set; }

        public ProcessPaymentRequest(Payment payment)
        {
            Require.NotNull(payment, "payment");
            Payment = payment;
        }
    }
}
