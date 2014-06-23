using Kooboo.Commerce.CMSIntegration.Plugins.Orders.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Payments.PayPal.CMSIntegration
{
    public class PayPalPaymentModel : PaymentModelBase
    {
        [Required]
        public string CreditCardNumber
        {
            get
            {
                return GetPaymentParameterValue("CreditCardNumber");
            }
            set
            {
                SetPaymentParameterValue("CreditCardNumber", value);
            }
        }

        [Required]
        public string CreditCardType
        {
            get
            {
                return GetPaymentParameterValue("CreditCardType");
            }
            set
            {
                SetPaymentParameterValue("CreditCardType", value);
            }
        }

        [Required]
        public int CreditCardExpireMonth
        {
            get
            {
                return GetPaymentParameterValue<int>("CreditCardExpireMonth");
            }
            set
            {
                SetPaymentParameterValue("CreditCardExpireMonth", value);
            }
        }

        [Required]
        public int CreditCardExpireYear
        {
            get
            {
                return GetPaymentParameterValue<int>("CreditCardExpireYear");
            }
            set
            {
                SetPaymentParameterValue("CreditCardExpireYear", value);
            }
        }

        [Required]
        public string CreditCardCvv2
        {
            get
            {
                return GetPaymentParameterValue("CreditCardCvv2");
            }
            set
            {
                SetPaymentParameterValue("CreditCardCvv2", value);
            }
        }
    }
}
