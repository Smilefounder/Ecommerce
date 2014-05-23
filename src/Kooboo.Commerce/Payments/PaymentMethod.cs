﻿using Kooboo.Commerce.ComponentModel;
using Kooboo.Commerce.Events;
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
        /// User specified unique id for this payment method.
        /// This is useful when refering a payment method in frontend cms websites.
        /// </summary>
        [Param]
        public string UniqueId { get; set; }

        [Required, StringLength(100)]
        public string DisplayName { get; set; }

        public string PaymentProcessorName { get; set; }

        public string PaymentProcessorData { get; set; }

        public PriceChangeMode AdditionalFeeChargeMode { get; set; }

        public decimal AdditionalFeeAmount { get; set; }

        public float AdditionalFeePercent { get; set; }

        public bool IsEnabled { get; protected set; }

        public DateTime CreatedAtUtc { get; set; }

        public PaymentMethod()
        {
            CreatedAtUtc = DateTime.UtcNow;
        }

        public virtual decimal GetPaymentMethodCost(decimal amountToPay)
        {
            if (AdditionalFeeChargeMode == PriceChangeMode.ByAmount)
            {
                return AdditionalFeeAmount;
            }

            return amountToPay * (decimal)AdditionalFeePercent;
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
