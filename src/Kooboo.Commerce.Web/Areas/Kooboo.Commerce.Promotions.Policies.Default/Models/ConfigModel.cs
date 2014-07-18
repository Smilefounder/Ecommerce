using Kooboo.Commerce.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Promotions.Policies.Default.Models
{
    public class ConfigModel
    {
        [Display(Name = "Discount mode")]
        public string DiscountMode { get; set; }

        public IList<SelectListItem> AvailableDiscountModes { get; set; }

        [Display(Name = "Discount percent")]
        public decimal DiscountPercent { get; set; }

        [Display(Name = "Discount amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Discount applied to")]
        public string DiscountAppliedTo { get; set; }

        public IList<SelectListItem> AvailableDiscountAppliedTos { get; set; }

        public ConfigModel()
        {
            AvailableDiscountModes = SelectListItems.FromEnum<DiscountMode>();
            AvailableDiscountAppliedTos = SelectListItems.FromEnum<DiscountAppliedTo>();
        }
    }
}