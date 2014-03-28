using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.AuthorizeNet
{
    public class AuthorizeNetConstants
    {
        public static readonly string CreditCardNumber = "CreditCardNumber";

        public static readonly string CreditCardType = "CreditCardType";

        public static readonly string CreditCardExpireMonth = "CreditCardExpireMonth";

        public static readonly string CreditCardExpireYear = "CreditCardExpireYear";

        public static readonly string CreditCardCvv2 = "CreditCardCvv2";

        public static readonly IEnumerable<PaymentProcessorParameterDescriptor> ParameterDescriptors = new List<PaymentProcessorParameterDescriptor>
        {
            new PaymentProcessorParameterDescriptor(CreditCardNumber) {
                IsRequired = true,
                Description = "Credit card number" 
            },
            new PaymentProcessorParameterDescriptor(CreditCardType) {
                IsRequired = true,
                Description = "Credit card type. e.g., visa, mastercard" 
            },
            new PaymentProcessorParameterDescriptor(CreditCardExpireMonth) {
                IsRequired = true,
                Description = "Credit card expire month. e.g., 08" 
            },
            new PaymentProcessorParameterDescriptor(CreditCardExpireYear) {
                IsRequired = true,
                Description = "Credit card expire year. e.g., 2018" 
            },
            new PaymentProcessorParameterDescriptor(CreditCardCvv2) {
                IsRequired = true,
                Description = "The three digits credit card cvv2 code." 
            }
        };
    }
}