using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [Event(Order = 300, ShortName = "Deleted")]
    public class BrandDeleted : BusinessEvent, IBrandEvent
    {
        [Param]
        public int BrandId { get; set; }

        [Param]
        public string BrandName { get; set; }

        protected BrandDeleted() { }

        public BrandDeleted(Brand brand)
        {
            BrandId = brand.Id;
            BrandName = brand.Name;
        }
    }
}
