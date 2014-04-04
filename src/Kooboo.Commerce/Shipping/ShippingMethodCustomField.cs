using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public class ShippingMethodCustomField
    {
        [Key, Column(Order = 0)]
        public int ShippingMethodId { get; set; }

        [Key, Column(Order = 1)]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
