using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Kooboo.Commerce.Products;
using System.ComponentModel.DataAnnotations.Schema;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Products
{
    public class ProductVariantField : IOrphanable, ICustomField
    {
        [Key]
        protected int Id { get; set; }

        [StringLength(50)]
        public string FieldName { get; set; }

        public string FieldValue { get; set; }

        [Column]
        protected int? ProductVariantId { get; set; }

        public ProductVariantField() { }

        public ProductVariantField(string fieldName, string fieldValue)
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
        }

        bool IOrphanable.IsOrphan()
        {
            return ProductVariantId == null;
        }

        public override string ToString()
        {
            return FieldName + " = " + FieldValue;
        }
    }
}
