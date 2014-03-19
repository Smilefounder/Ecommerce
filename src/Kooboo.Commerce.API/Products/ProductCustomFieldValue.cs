using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.EAV;

namespace Kooboo.Commerce.API.Products
{
    public class ProductCustomFieldValue
    {
        public int ProductId { get; set; }
        public int CustomFieldId { get; set; }
        public string FieldValue { get; set; }

        public string FieldText { get; set; }

        public Product Product { get; set; }
        public CustomField CustomField { get; set; }
    }
}
