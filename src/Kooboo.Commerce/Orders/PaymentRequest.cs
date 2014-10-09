using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders
{
    public class PaymentRequest
    {
        public int OrderId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string ReturnUrl { get; set; }

        public string CurrencyCode { get; set; }

        public IDictionary<string, string> Parameters { get; set; }

        public PaymentRequest()
        {
            Parameters = new Dictionary<string, string>();
        }
    }
}
