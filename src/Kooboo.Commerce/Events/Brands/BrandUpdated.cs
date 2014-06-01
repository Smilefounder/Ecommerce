using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [Event(Order = 200)]
    public class BrandUpdated : DomainEvent, IBrandEvent
    {
        [Reference(typeof(Brand))]
        public int BrandId { get; set; }

        public string BrandName { get; set; }

        public string BrandDescription { get; set; }

        public BrandUpdated() { }

        public BrandUpdated(Brand brand)
        {
            BrandId = brand.Id;
            BrandName = brand.Name;
            BrandDescription = brand.Description;
        }
    }
}
