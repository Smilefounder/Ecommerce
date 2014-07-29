using Kooboo.Commerce.Settings;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings
{
    [MetadataFor(typeof(GlobalSettings))]
    public class GlobalSettings_Metadata
    {
        [UIHint("DropDownList")]
        [DataSource(typeof(CurrencyDataSource))]
        public string Currency { get; set; }

        [Required]
        [Display(Name = "Weight unit name")]
        public string WeightUnitName { get; set; }

        [Required]
        [Display(Name = "Size unit name")]
        public string SizeUnitName { get; set; }
    }
}