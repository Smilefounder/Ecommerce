using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products.Services
{
    public class CreateProductTypeRequest
    {
        public string Name { get; set; }

        public string SkuAlias { get; set; }

        public List<CustomField> CustomFields { get; set; }

        public List<CustomField> VariantFields { get; set; }
    }
}
