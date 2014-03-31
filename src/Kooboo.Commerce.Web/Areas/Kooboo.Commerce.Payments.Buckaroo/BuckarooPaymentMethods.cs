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

            public static readonly IEnumerable<PaymentProcessorParameterDescriptor> Descriptors = new List<PaymentProcessorParameterDescriptor>
            {
                new PaymentProcessorParameterDescriptor(ServiceId) 
                {
                    IsRequired = true,
                    Description = "The buckaroo payment service to use."
                },
                new PaymentProcessorParameterDescriptor(SEPA_CustomerAccountName)
                {
                    IsRequired = false,
                    Description = "SEPA customer account name. Required for SEPA service."
                },
                new PaymentProcessorParameterDescriptor(SEPA_CustomerBIC)
                {
                    IsRequired = false,
                    Description = "SEPA customer BIC. Required for SEPA service."
                },
                new PaymentProcessorParameterDescriptor(SEPA_CustomerIBAN)
                {
                    IsRequired = false,
                    Description = "SEPA customer IBAN. Required for SEPA service."
                }
            };
        }
    }
}