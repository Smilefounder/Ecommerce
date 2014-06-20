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
    public class PaymentProcessingContext
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

        public string ReturnUrl { get; set; }

        public IDictionary<string, string> Parameters { get; set; }

        public object ProcessorConfig { get; set; }

        public PaymentProcessingContext(Payment payment, object processorConfig)
        {
            Require.NotNull(payment, "payment");
            Payment = payment;
            ProcessorConfig = processorConfig;
            Parameters = new Dictionary<string, string>();
        }
    }
}
