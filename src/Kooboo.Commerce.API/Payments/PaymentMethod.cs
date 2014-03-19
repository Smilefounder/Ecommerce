using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public PaymentMethodType Type { get; set; }

        public string PaymentProcessorName { get; set; }

        /// <summary>
        /// Represents the corresponding payment method id in the payment processor system.
        /// One payment processor can support multiple payment methods.
        /// Each payment method has an id which might be specific to the payment processor.
        /// </summary>
        public string PaymentProcessorMethodId { get; set; }

        public PriceChangeMode AdditionalFeeChargeMode { get; set; }

        public decimal AdditionalFeeAmount { get; set; }

        public float AdditionalFeePercent { get; set; }

        public bool IsEnabled { get; protected set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
