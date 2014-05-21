using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.Buckaroo
{
    public static class BuckarooConstants
    {
        public static class Services
        {
            public static readonly string Amex = "amex";

            public static readonly string Visa = "visa";

            public static readonly string MasterCard = "mastercard";

            public static readonly string SimpleSEPADirectDebit = "simplesepadirectdebit";

            public static readonly string iDeal = "ideal";

            public static readonly string PayPal = "paypal";
        }

        public static class Parameters
        {
            public static readonly string ServiceId = "ServiceId";

            public static readonly string SEPA_CustomerAccountName = "SEPA_CustomerAccountName";

            public static readonly string SEPA_CustomerBIC = "SEPA_CustomerBIC";

            public static readonly string SEPA_CustomerIBAN = "SEPA_CustomerIBAN";
        }
    }
}