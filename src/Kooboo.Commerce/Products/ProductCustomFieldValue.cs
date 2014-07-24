using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kooboo.Commerce.EAV;

namespace Kooboo.Commerce.Products
{
    public class ProductCustomFieldValue
    {
        [Key, Column(Order = 0)]
        public int ProductId { get; set; }
        [Key, Column(Order = 1)]
        public int CustomFieldId { get; set; }

        public string FieldValue { get; set; }

        public virtual Product Product { get; set; }

        public virtual CustomField CustomField { get; set; }

        public ProductCustomFieldValue() { }

        public ProductCustomFieldValue(Product product, int fieldId, string value)
        {
            Product = product;
            CustomFieldId = fieldId;
            FieldValue = value;
        }
    }
}
