using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.PaymentMethods
{
    public class PaymentProcessorModel
    {
        public string Name { get; set; }
    }

    public class PaymentMethodEditorModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Payment processor")]
        public string PaymentProcessorName { get; set; }

        [Display(Name = "Unique ID")]
        [Description("Optional user defined unique id for referencing in cms frontend websites")]
        public string UniqueId { get; set; }

        [Display(Name = "Additional fee charge mode")]
        public PriceChangeMode AdditionalFeeChargeMode { get; set; }

        public IList<SelectListItem> AllFeeChargeModes { get; set; }

        [Display(Name = "Additional fee amount")]
        public decimal AdditionalFeeAmount { get; set; }

        [Range(0, 100)]
        [Display(Name = "Additional fee percent")]
        public float AdditionalFeePercent { get; set; }

        public IList<PaymentProcessorModel> AvailablePaymentProcessors { get; set; }

        public PaymentMethodEditorModel()
        {
            AvailablePaymentProcessors = new List<PaymentProcessorModel>();
            AllFeeChargeModes = SelectListItems.FromEnum(typeof(PriceChangeMode));
        }

        public void UpdateTo(PaymentMethod method)
        {
            method.DisplayName = DisplayName;
            method.UniqueId = UniqueId;
            method.PaymentProcessorName = PaymentProcessorName;
            method.AdditionalFeeChargeMode = AdditionalFeeChargeMode;
            method.AdditionalFeeAmount = AdditionalFeeAmount;
            method.AdditionalFeePercent = AdditionalFeePercent;
        }
    }
}