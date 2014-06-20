using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Shipping.FixedRate
{
    public class FixedShippingRateProviderConfig
    {
        [Required]
        [Display(Name = "Shipping rate")]
        public decimal ShippingRate { get; set; }
    }
}
