using Kooboo.Commerce.ComponentModel;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.PaymentMethods;
using Kooboo.Commerce.Events.Payments;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class PaymentMethod
    {
        [Param]
        public int Id { get; set; }

        /// <summary>
        /// User specified key for this payment method.
        /// This is useful when refering a payment method in frontend cms websites.
        /// </summary>
        [Param]
        public string UserKey { get; set; }

        [Param]
        [Required, StringLength(100)]
        public string Name { get; set; }

        public string PaymentProcessorName { get; set; }

        public string PaymentProcessorData { get; set; }

        public PaymentMethodFeeChargeMode AdditionalFeeChargeMode { get; set; }

        public decimal AdditionalFeeAmount { get; set; }

        public decimal AdditionalFeePercent { get; set; }

        public bool IsEnabled { get; protected set; }

        public DateTime CreatedAtUtc { get; set; }

        public PaymentMethod()
        {
            CreatedAtUtc = DateTime.UtcNow;
        }

        public virtual decimal GetPaymentMethodCost(decimal amountToPay)
        {
            if (AdditionalFeeChargeMode == PaymentMethodFeeChargeMode.ByAmount)
            {
                return AdditionalFeeAmount;
            }

            return Math.Round(amountToPay * (AdditionalFeePercent / 100), 2);
        }

        public virtual void NotifyUpdated()
        {
            Event.Raise(new PaymentMethodUpdated(this));
        }

        public virtual bool MarkEnabled()
        {
            if (!IsEnabled)
            {
                IsEnabled = true;
                return true;
            }

            return false;
        }

        public virtual bool MarkDisabled()
        {
            if (IsEnabled)
            {
                IsEnabled = false;
                return true;
            }

            return false;
        }
    }
}
