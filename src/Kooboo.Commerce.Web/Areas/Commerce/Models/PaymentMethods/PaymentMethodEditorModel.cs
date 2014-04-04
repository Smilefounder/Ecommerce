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

        public IList<NameValue> CustomFields { get; set; }

        public PaymentMethodEditorModel()
        {
            AvailablePaymentProcessors = new List<PaymentProcessorModel>();
            AllFeeChargeModes = EnumUtil.ToSelectList(typeof(PriceChangeMode));
            CustomFields = new List<NameValue>();
        }

        public void UpdateSimplePropertiesTo(PaymentMethod method)
        {
            method.DisplayName = DisplayName;
            method.UniqueId = UniqueId;
            method.PaymentProcessorName = PaymentProcessorName;
            method.AdditionalFeeChargeMode = AdditionalFeeChargeMode;
            method.AdditionalFeeAmount = AdditionalFeeAmount;
            method.AdditionalFeePercent = AdditionalFeePercent;
        }

        public void UpdateCustomFieldsTo(PaymentMethod method)
        {
            method.CustomFields.Clear();

            foreach (var field in CustomFields)
            {
                method.CustomFields.Add(new PaymentMethodCustomField
                {
                    PaymentMethodId = method.Id,
                    Name = field.Name,
                    Value = field.Value
                });
            }
        }
    }
}