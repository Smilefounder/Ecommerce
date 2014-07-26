using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Categories
{
    public class CategoryCustomField
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual Category Category { get; set; }
    }
}
