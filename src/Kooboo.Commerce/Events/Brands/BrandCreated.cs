using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    public class BrandCreated : IBrandEvent
    {
        [Reference(typeof(Brand))]
        public int BrandId { get; set; }

        public BrandCreated() { }

        public BrandCreated(Brand brand)
        {
            BrandId = brand.Id;
        }
    }
}
