using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight.Models
{
    public class ByWeightShippingRuleModel
    {
        [Required]
        public string FromWeight { get; set; }

        [Required]
        public string ToWeight { get; set; }

        [Required]
        public string ShippingPrice { get; set; }

        public string PriceUnit { get; set; }
    }
}