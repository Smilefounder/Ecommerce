using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Kooboo.Commerce.EAV;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kooboo.Commerce.Products
{
    public class ProductVariantFieldValue
    {
        [Key, Column(Order = 0)]
        public int ProductPriceId { get; set; }

        [Key, Column(Order = 1)]
        public int CustomFieldId { get; set; }

        public string FieldText { get; set; }

        public string FieldValue { get; set; }

        public virtual ProductVariant ProductPrice { get; set; }

        public virtual CustomField CustomField { get; set; }

        public ProductVariantFieldValue() { }

        public ProductVariantFieldValue(ProductVariant price, int fieldId, string value)
        {
            ProductPrice = price;
            CustomFieldId = fieldId;
            FieldValue = value;
        }
    }
}
