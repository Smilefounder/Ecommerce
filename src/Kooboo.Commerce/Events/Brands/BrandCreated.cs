using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [Serializable]
    public class BrandCreated : Event, IBrandEvent
    {
        [ConditionParameter]
        public int BrandId { get; set; }

        [ConditionParameter]
        public string BrandName { get; set; }

        [ConditionParameter]
        public string BrandDescription { get; set; }

        public BrandCreated() { }

        public BrandCreated(Brand brand)
        {
            BrandId = brand.Id;
            BrandName = brand.Name;
            BrandDescription = brand.Description;
        }
    }
}
