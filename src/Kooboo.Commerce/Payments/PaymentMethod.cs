using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(100)]
        public string PaymentGatewayName { get; set; }

        public string PaymentGatewayData { get; set; }

        public PriceChangeMode AdditionalFeeChargeMode { get; set; }

        public decimal AdditionalFeeAmount { get; set; }

        public float AdditionalFeePercent { get; set; }

        public bool IsEnabled { get; protected set; }

        public DateTime CreatedAtUtc { get; set; }

        public PaymentMethod()
        {
            CreatedAtUtc = DateTime.UtcNow;
        }

        public virtual decimal GetPaymentMethodFee(decimal sutotal)
        {
            if (AdditionalFeeChargeMode == PriceChangeMode.ByAmount)
            {
                return AdditionalFeeAmount;
            }

            return sutotal * (decimal)AdditionalFeePercent;
        }

        public virtual void Enable()
        {
            if (!IsEnabled)
            {
                IsEnabled = true;
            }
        }

        public virtual void Disable()
        {
            if (IsEnabled)
            {
                IsEnabled = false;
            }
        }
    }
}
