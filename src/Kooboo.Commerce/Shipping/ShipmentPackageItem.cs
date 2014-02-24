using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public class ShipmentPackageItem
    {
        public int ProductId { get; set; }

        public int ProductVariantId { get; set; }

        public int Quantity { get; set; }
    }
}
