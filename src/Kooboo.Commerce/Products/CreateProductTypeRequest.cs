using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    public class CreateProductTypeRequest
    {
        public string Name { get; set; }

        public string SkuAlias { get; set; }

        public List<CustomFieldDefinition> CustomFields { get; set; }

        public List<CustomFieldDefinition> VariantFields { get; set; }
    }
}
