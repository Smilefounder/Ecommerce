using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [ActivityVisible("Brand Events")]
    public class BrandCreated : IEvent
    {
        public Brand Brand { get; private set; }

        public BrandCreated(Brand brand)
        {
            Brand = brand;
        }
    }
}
