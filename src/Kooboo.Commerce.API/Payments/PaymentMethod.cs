using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserKey { get; set; }

        public PriceChangeMode AdditionalFeeChargeMode { get; set; }

        public decimal AdditionalFeeAmount { get; set; }

        public float AdditionalFeePercent { get; set; }

        public string PaymentProcessorName { get; set; }

        public decimal GetPaymentMethodFee(decimal total)
        {
            if (AdditionalFeeChargeMode == PriceChangeMode.ByAmount)
            {
                return AdditionalFeeAmount;
            }

            return Math.Round((decimal)AdditionalFeePercent * total, 2);
        }
    }
}
