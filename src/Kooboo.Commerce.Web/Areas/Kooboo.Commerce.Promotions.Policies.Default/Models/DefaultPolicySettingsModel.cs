using Kooboo.Commerce.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Promotions.Policies.Default.Models
{
    public class DefaultPolicySettingsModel
    {
        [Display(Name = "Discount mode")]
        public PriceChangeMode DiscountMode { get; set; }

        public IList<SelectListItem> AvailableDiscountModes { get; set; }

        [Display(Name = "Discount percent")]
        public float DiscountPercent { get; set; }

        [Display(Name = "Discount amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Discount applied to")]
        public DiscountAppliedTo DiscountAppliedTo { get; set; }

        public IList<SelectListItem> AvailableDiscountAppliedTos { get; set; }

        public DefaultPolicySettingsModel()
        {
            AvailableDiscountModes = EnumUtil.ToSelectList<PriceChangeMode>();
            AvailableDiscountAppliedTos = EnumUtil.ToSelectList<DiscountAppliedTo>();
        }
    }
}