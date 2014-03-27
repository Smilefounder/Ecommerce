using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.PayPal
{
    public static class PayPalParameters
    {
        public static readonly string CreditCardNumber = "CreditCardNumber";

        public static readonly string CreditCardType = "CreditCardType";

        public static readonly string CreditCardExpireMonth = "CreditCardExpireMonth";

        public static readonly string CreditCardExpireYear = "CreditCardExpireYear";

        public static readonly string CreditCardCvv2 = "CreditCardCvv2";

        public static readonly IDictionary<string, string> ParameterDescriptions = new Dictionary<string, string>
        {
            { CreditCardNumber, "Credit card number" },
            { CreditCardType, "Credit card type. e.g., visa, mastercard" },
            { CreditCardExpireMonth, "Credit card expire month. e.g., 08" },
            { CreditCardExpireYear, "Credit card expire year. e.g., 2018" },
            { CreditCardCvv2, "The three digits credit card cvv2 code." }
        };
    }
}