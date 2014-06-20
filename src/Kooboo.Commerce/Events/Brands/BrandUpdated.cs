using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [Event(Order = 200, ShortName = "Updated")]
    public class BrandUpdated : BusinessEvent, IBrandEvent
    {
        [Reference(typeof(Brand))]
        public int BrandId { get; set; }

        protected BrandUpdated() { }

        public BrandUpdated(Brand brand)
        {
            BrandId = brand.Id;
        }
    }
}
