using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.PaymentMethods
{
    public class PaymentProcessorModel
    {
        public string Name { get; set; }

        public IList<SupportedPaymentMethod> SupportedPaymentMethods { get; set; }

        public PaymentProcessorModel()
        {
            SupportedPaymentMethods = new List<SupportedPaymentMethod>();
        }
    }

    public class PaymentMethodEditorModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Payment type")]
        public PaymentMethodType PaymentType { get; set; }

        public IList<SelectListItem> AllPaymentTypes { get; set; }

        [Required]
        [Display(Name = "Payment processor")]
        public string PaymentProcessorName { get; set; }

        public string PaymentProcessorMethodId { get; set; }

        [Display(Name = "Additional fee charge mode")]
        public PriceChangeMode AdditionalFeeChargeMode { get; set; }

        public IList<SelectListItem> AllFeeChargeModes { get; set; }

        [Display(Name = "Additional fee amount")]
        public decimal AdditionalFeeAmount { get; set; }

        [Range(0, 100)]
        [Display(Name = "Additional fee percent")]
        public float AdditionalFeePercent { get; set; }

        public IList<PaymentProcessorModel> AvailablePaymentProcessors { get; set; }

        public IList<SupportedPaymentMethod> AvailablePaymentMethods { get; set; }

        [Display(Name = "Enabled")]
        public bool IsEnabled { get; set; }

        public bool IsEdit { get; set; }

        public PaymentMethodEditorModel()
        {
            AvailablePaymentProcessors = new List<PaymentProcessorModel>();
            AllPaymentTypes = EnumUtil.ToSelectList(typeof(PaymentMethodType));
            AvailablePaymentMethods = new List<SupportedPaymentMethod>();
            AllFeeChargeModes = EnumUtil.ToSelectList(typeof(PriceChangeMode));
        }

        public void UpdateTo(PaymentMethod method)
        {
            method.DisplayName = DisplayName;
            method.PaymentProcessorName = PaymentProcessorName;
            method.PaymentProcessorMethodId = PaymentProcessorMethodId;
            method.AdditionalFeeChargeMode = AdditionalFeeChargeMode;
            method.AdditionalFeeAmount = AdditionalFeeAmount;
            method.AdditionalFeePercent = AdditionalFeePercent;
        }
    }
}