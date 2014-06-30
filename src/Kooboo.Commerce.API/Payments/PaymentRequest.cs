using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    /// <summary>
    /// the request that asking for creating a payment
    /// </summary>
    public class PaymentRequest
    {
        public int OrderId { get; set; }

        /// <summary>
        /// description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// total amount that need to pay
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// payment method id
        /// </summary>
        public int PaymentMethodId { get; set; }
        /// <summary>
        /// return url when payment complete.
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// The currency code.
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// The parameters required by the payment processor to complete the payment.
        /// It's often used in direct payment senario.
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; }

        public PaymentRequest()
        {
            Parameters = new Dictionary<string, string>();
        }
    }
}
