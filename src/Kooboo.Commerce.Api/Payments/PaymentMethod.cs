using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Payments
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserKey { get; set; }

        public PaymentMethodFeeChargeMode AdditionalFeeChargeMode { get; set; }

        public decimal AdditionalFeeAmount { get; set; }

        public decimal AdditionalFeePercent { get; set; }

        public decimal GetPaymentMethodFee(decimal total)
        {
            if (AdditionalFeeChargeMode == PaymentMethodFeeChargeMode.ByAmount)
            {
                return AdditionalFeeAmount;
            }

            return Math.Round((decimal)AdditionalFeePercent * total, 2);
        }
    }
}
