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
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Payment Processor")]
        public string PaymentProcessorName { get; set; }

        [Display(Name = "User Key")]
        [Description("Optional user defined unique id for referencing in cms frontend websites")]
        public string UserKey { get; set; }

        [Display(Name = "Additional fee charge mode")]
        public PaymentMethodFeeChargeMode AdditionalFeeChargeMode { get; set; }

        public IList<SelectListItem> AllFeeChargeModes { get; set; }

        [Display(Name = "Additional fee amount")]
        public decimal AdditionalFeeAmount { get; set; }

        [Range(0, 100)]
        [Display(Name = "Additional fee percent")]
        public decimal AdditionalFeePercent { get; set; }

        public IList<PaymentProcessorModel> AvailablePaymentProcessors { get; set; }

        public PaymentMethodEditorModel()
        {
            AvailablePaymentProcessors = new List<PaymentProcessorModel>();
            AllFeeChargeModes = SelectListItems.FromEnum(typeof(PaymentMethodFeeChargeMode));
        }

        public void UpdateTo(PaymentMethod method)
        {
            method.Name = Name;
            method.UserKey = UserKey;
            method.PaymentProcessorName = PaymentProcessorName;
            method.AdditionalFeeChargeMode = AdditionalFeeChargeMode;
            method.AdditionalFeeAmount = AdditionalFeeAmount;
            method.AdditionalFeePercent = AdditionalFeePercent;
        }
    }
}