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
    public class PaymentMethodEditorModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Payment gateway")]
        public string PaymentGateway { get; set; }

        [Display(Name = "Additional fee charge mode")]
        public PriceChangeMode AdditionalFeeChargeMode { get; set; }

        [Display(Name = "Additional fee amount")]
        public decimal AdditionalFeeAmount { get; set; }

        [Range(0, 100)]
        [Display(Name = "Additional fee percent")]
        public float AdditionalFeePercent { get; set; }

        public IList<SelectListItem> AvailableGateways { get; set; }

        public IList<SelectListItem> AvailableAdditionalFeeChargeModes { get; set; }

        [Display(Name = "Enabled")]
        public bool IsEnabled { get; set; }

        public bool IsEdit { get; set; }

        public PaymentMethodEditorModel()
        {
            AvailableAdditionalFeeChargeModes = new List<SelectListItem>();
            AvailableAdditionalFeeChargeModes = EnumUtil.ToSelectList(typeof(PriceChangeMode));
        }

        public void UpdateTo(PaymentMethod method)
        {
            method.DisplayName = DisplayName;
            method.PaymentGatewayName = PaymentGateway;
            method.AdditionalFeeChargeMode = AdditionalFeeChargeMode;
            method.AdditionalFeeAmount = AdditionalFeeAmount;
            method.AdditionalFeePercent = AdditionalFeePercent;
        }
    }
}