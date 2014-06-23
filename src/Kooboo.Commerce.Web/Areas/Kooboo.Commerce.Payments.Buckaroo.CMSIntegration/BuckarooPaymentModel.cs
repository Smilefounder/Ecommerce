using Kooboo.Commerce.CMSIntegration.Plugins.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Payments.Buckaroo.CMSIntegration
{
    public class BuckarooPaymentModel : PaymentModelBase
    {
        public string CustomerAccountName
        {
            get
            {
                return GetPaymentParameterValue("CustomerAccountName");
            }
            set
            {
                SetPaymentParameterValue("CustomerAccountName", value);
            }
        }

        public string CustomerBIC
        {
            get
            {
                return GetPaymentParameterValue("CustomerBIC");
            }
            set
            {
                SetPaymentParameterValue("CustomerBIC", value);
            }
        }

        public string CustomerIBAN
        {
            get
            {
                return GetPaymentParameterValue("CustomerIBAN");
            }
            set
            {
                SetPaymentParameterValue("CustomerIBAN", value);
            }
        }
    }
}
