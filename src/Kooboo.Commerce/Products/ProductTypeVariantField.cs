using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kooboo.Commerce.Products
{
    public class ProductTypeVariantField
    {
        [Key, Column(Order = 0)]
        public int ProductTypeId { get; set; }
        [Key, Column(Order = 1)]
        public int CustomFieldId { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual CustomField CustomField { get; set; }

        public ProductTypeVariantField() { }

        public ProductTypeVariantField(ProductType productType, CustomField field)
        {
            ProductType = productType;
            CustomField = field;
        }
    }
}
