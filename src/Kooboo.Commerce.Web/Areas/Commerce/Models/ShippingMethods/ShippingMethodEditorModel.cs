using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ShippingMethods
{
    public class ShippingMethodEditorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Shipping rate provider")]
        public string ShippingRateProviderName { get; set; }

        public IList<SelectListItem> AvailableShippingRateProviders { get; set; }
    }
}