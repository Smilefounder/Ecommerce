using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [Event(Order = 300)]
    public class BrandDeleted : DomainEvent, IBrandEvent
    {
        [Param]
        public int BrandId { get; set; }

        [Param]
        public string BrandName { get; set; }

        [Param]
        public string BrandDescription { get; set; }

        public BrandDeleted() { }

        public BrandDeleted(Brand brand)
        {
            BrandId = brand.Id;
            BrandName = brand.Name;
            BrandDescription = brand.Description;
        }
    }
}
