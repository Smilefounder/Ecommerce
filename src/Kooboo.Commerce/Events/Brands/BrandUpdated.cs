using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    public class BrandUpdated : IBrandEvent
    {
        [Reference(typeof(Brand))]
        public int BrandId { get; set; }

        public BrandUpdated() { }

        public BrandUpdated(Brand brand)
        {
            BrandId = brand.Id;
        }
    }
}
