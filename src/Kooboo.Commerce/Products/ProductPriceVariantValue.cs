using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kooboo.Commerce.EAV;

namespace Kooboo.Commerce.Products
{
    public class ProductPriceVariantValue
    {
        [Key, Column(Order = 0)]
        public int ProductPriceId { get; set; }

        [Key, Column(Order = 1)]
        public int CustomFieldId { get; set; }

        public string FieldValue { get; set; }

        public string FieldText { get; set; }

        public virtual ProductPrice ProductPrice { get; set; }

        public virtual CustomField CustomField { get; set; }

        public ProductPriceVariantValue() { }

        public ProductPriceVariantValue(ProductPrice price, int fieldId, string value)
        {
            ProductPrice = price;
            CustomFieldId = fieldId;
            FieldValue = value;
        }
    }
}
