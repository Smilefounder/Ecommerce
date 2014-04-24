using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [Serializable]
    public class BrandUpdated : Event, IBrandEvent
    {
        [Parameter]
        public int BrandId { get; set; }

        [Parameter]
        public string BrandName { get; set; }

        [Parameter]
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
