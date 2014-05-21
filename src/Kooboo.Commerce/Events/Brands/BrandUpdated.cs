using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [Serializable]
    public class BrandUpdated : DomainEvent, IBrandEvent
    {
        [ConditionParameter]
        public int BrandId { get; set; }

        [ConditionParameter]
        public string BrandName { get; set; }

        [ConditionParameter]
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
