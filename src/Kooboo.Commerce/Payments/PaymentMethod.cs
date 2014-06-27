using Kooboo.Commerce.Rules;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

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

        [Required, StringLength(100)]
        public string ProcessorName { get; set; }

        private string ProcessorConfig { get; set; }

        public T LoadProcessorConfig<T>()
            where T : class
        {
            return LoadProcessorConfig(typeof(T)) as T;
        }

        public object LoadProcessorConfig(Type configModelType)
        {
            if (String.IsNullOrWhiteSpace(ProcessorConfig))
            {
                return null;
            }

            return JsonConvert.DeserializeObject(ProcessorConfig, configModelType);
        }

        public void UpdateProcessorConfig(object configModel)
        {
            if (configModel == null)
            {
                ProcessorConfig = null;
            }
            else
            {
                ProcessorConfig = JsonConvert.SerializeObject(configModel);
            }
        }

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

        #region Entity Mapping

        class PaymentMethodMap : EntityTypeConfiguration<PaymentMethod>
        {
            public PaymentMethodMap()
            {
                Property(c => c.ProcessorConfig);
            }
        }

        #endregion
    }
}
