using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public enum PaymentType
    {
        /// <summary>
        /// Customers are redirected to third-party website to complete the payment.
        /// </summary>
        ExternalPayment = 0,

        /// <summary>
        /// Complete payment directly by providing credit card info.
        /// </summary>
        CreditCard = 1,

        /// <summary>
        /// Complete payment directly by provider bank account info.
        /// </summary>
        DirectDebit = 2
    }
}
