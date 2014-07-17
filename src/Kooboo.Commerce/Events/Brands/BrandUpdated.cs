using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [ActivityEvent(Order = 200)]
    public class BrandUpdated : Event, IBrandEvent
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
