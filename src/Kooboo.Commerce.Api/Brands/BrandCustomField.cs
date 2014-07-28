using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Brands
{
    public class BrandCustomField
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
