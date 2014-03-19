using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public enum PaymentMethodType
    {
        /// <summary>
        /// Customers are redirected to third-party website to complete the payment.
        /// </summary>
        [Description("External Payment")]
        ExternalPayment = 0,

        /// <summary>
        /// Complete payment directly by providing credit card info.
        /// </summary>
        [Description("Credit Card")]
        CreditCard = 1,

        /// <summary>
        /// Complete payment directly by provider bank account info.
        /// </summary>
        [Description("Direct Debit")]
        DirectDebit = 2
    }
}
