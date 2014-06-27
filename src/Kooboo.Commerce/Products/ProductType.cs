using Kooboo.Commerce.Rules;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Kooboo.Commerce.Products
{

    /// <summary>
    /// Define the type of a product, the type defines all the custom properties and variations. 
    /// </summary>
    public class ProductType
    {
        [Key, Param]
        public int Id { get; set; }

        [Param]
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Param]
        [Required, StringLength(50)]
        public string SkuAlias { get; set; }

        [Param]
        public bool IsEnabled { get; set; }

        public virtual ICollection<ProductTypeCustomField> CustomFields { get; set; }

        public virtual ICollection<ProductTypeVariantField> VariationFields { get; set; }

        public ProductType()
        {
            CustomFields = new List<ProductTypeCustomField>();
            VariationFields = new List<ProductTypeVariantField>();
        }

        public ProductType(string name, string skuAlias = "SKU")
            : this()
        {
            Name = name;
            SkuAlias = skuAlias;
        }

        public virtual ProductTypeCustomField FindCustomField(int fieldId)
        {
            return CustomFields.FirstOrDefault(f => f.CustomFieldId == fieldId);
        }

        public virtual bool RemoveCustomField(int fieldId)
        {
            var field = CustomFields.FirstOrDefault(f => f.CustomFieldId == fieldId);
            if (field != null)
            {
                CustomFields.Remove(field);
                return true;
            }

            return false;
        }

        public virtual ProductTypeVariantField FindVariantField(int fieldId)
        {
            return VariationFields.FirstOrDefault(f => f.CustomFieldId == fieldId);
        }

        public virtual bool RemoveVariantField(int fieldId)
        {
            var field = VariationFields.FirstOrDefault(f => f.CustomFieldId == fieldId);
            if (field != null)
            {
                VariationFields.Remove(field);
                return true;
            }

            return false;
        }
    }
}
