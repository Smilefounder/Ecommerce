using Kooboo.Commerce.EAV;
using Kooboo.Commerce.Products.Internal;
using Kooboo.Commerce.Rules;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
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

        protected virtual ICollection<ProductTypeCustomField> InternalCustomFields { get; set; }
        private CustomFieldCollection _customFields;

        public CustomFieldCollection CustomFields
        {
            get
            {
                if (_customFields == null)
                {
                    var orderedFields = InternalCustomFields.OrderBy(f => f.Sequence).Select(f => f.CustomField);
                    _customFields = new CustomFieldCollection(orderedFields, OnCustomFieldAdded, OnCustomFieldRemoved, OnCustomFieldsSorted);
                }
                return _customFields;
            }
        }

        protected virtual ICollection<ProductTypeVariantField> InternalVariantFields { get; set; }
        private CustomFieldCollection _variantFields;

        public CustomFieldCollection VariantFields
        {
            get
            {
                if (_variantFields == null)
                {
                    var orderedFields = InternalVariantFields.OrderBy(f => f.Sequence).Select(f => f.CustomField);
                    _variantFields = new CustomFieldCollection(orderedFields, OnVariantFieldAdded, OnVariantFieldRemoved, OnVariantFieldsSorted);
                }
                return _variantFields;
            }
        }

        public ProductType()
        {
            InternalCustomFields = new List<ProductTypeCustomField>();
            InternalVariantFields = new List<ProductTypeVariantField>();
        }

        public ProductType(string name, string skuAlias = "SKU")
            : this()
        {
            Name = name;
            SkuAlias = skuAlias;
        }

        #region Custom field collection callbacks

        private void OnCustomFieldAdded(CustomField field)
        {
            InternalCustomFields.Add(new ProductTypeCustomField
            {
                CustomField = field,
                Sequence = InternalCustomFields.Count == 0 ? 0 : InternalCustomFields.Max(f => f.Sequence) + 1
            });
        }

        private void OnCustomFieldRemoved(CustomField field)
        {
            var internalField = InternalCustomFields.FirstOrDefault(f => f.CustomField.Name == field.Name);
            if (internalField != null)
            {
                InternalCustomFields.Remove(internalField);
            }
        }

        private void OnCustomFieldsSorted()
        {
            var sequence = 0;
            foreach (var field in CustomFields)
            {
                var internalField = InternalCustomFields.FirstOrDefault(f => f.CustomField.Name == field.Name);
                internalField.Sequence = sequence;
                sequence++;
            }
        }

        #endregion

        #region Variant field collection callbacks

        private void OnVariantFieldAdded(CustomField field)
        {
            InternalVariantFields.Add(new ProductTypeVariantField
            {
                CustomField = field,
                Sequence = InternalVariantFields.Count == 0 ? 0 : InternalVariantFields.Max(f => f.Sequence) + 1
            });
        }

        private void OnVariantFieldRemoved(CustomField field)
        {
            var internalField = InternalVariantFields.FirstOrDefault(f => f.CustomField.Name == field.Name);
            if (internalField != null)
            {
                InternalVariantFields.Remove(internalField);
            }
        }

        private void OnVariantFieldsSorted()
        {
            var sequence = 0;
            foreach (var field in VariantFields)
            {
                var internalField = InternalVariantFields.FirstOrDefault(f => f.CustomField.Name == field.Name);
                internalField.Sequence = sequence;
                sequence++;
            }
        }

        #endregion

        class ProductTypeConfiguration : EntityTypeConfiguration<ProductType>
        {
            public ProductTypeConfiguration()
            {
                HasMany(c => c.InternalCustomFields).WithRequired(c => c.ProductType);
                HasMany(c => c.InternalVariantFields).WithRequired(c => c.ProductType);
            }
        }
    }
}
