using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Internal;
using Kooboo.Commerce.Rules;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Kooboo.Commerce.Products
{

    /// <summary>
    /// Define the type of a product, the type defines all the custom properties and variations. 
    /// </summary>
    public class ProductType : ILocalizable
    {
        [Key, Param]
        public int Id { get; set; }

        [Param]
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Param]
        [StringLength(50)]
        public string SkuAlias { get; set; }

        [Param]
        public bool IsEnabled { get; set; }

        protected virtual ICollection<ProductTypeCustomFieldDefinition> InternalCustomFieldDefinitions { get; set; }
        private OrderedCustomFieldDefinitionCollection _customFieldDefinitions;

        [NotMapped]
        public OrderedCustomFieldDefinitionCollection CustomFieldDefinitions
        {
            get
            {
                if (_customFieldDefinitions == null)
                {
                    var orderedFields = InternalCustomFieldDefinitions.OrderBy(f => f.Sequence).Select(f => f.CustomField);
                    _customFieldDefinitions = new OrderedCustomFieldDefinitionCollection(orderedFields, OnCustomFieldAdded, OnCustomFieldRemoved, OnCustomFieldsSorted);
                }
                return _customFieldDefinitions;
            }
        }

        protected virtual ICollection<ProductTypeVariantFieldDefintion> InternalVariantFieldDefinitions { get; set; }
        private OrderedCustomFieldDefinitionCollection _variantFieldDefinitions;

        [NotMapped]
        public OrderedCustomFieldDefinitionCollection VariantFieldDefinitions
        {
            get
            {
                if (_variantFieldDefinitions == null)
                {
                    var orderedFields = InternalVariantFieldDefinitions.OrderBy(f => f.Sequence).Select(f => f.CustomField);
                    _variantFieldDefinitions = new OrderedCustomFieldDefinitionCollection(orderedFields, OnVariantFieldAdded, OnVariantFieldRemoved, OnVariantFieldsSorted);
                }
                return _variantFieldDefinitions;
            }
        }

        public ProductType()
        {
            InternalCustomFieldDefinitions = new List<ProductTypeCustomFieldDefinition>();
            InternalVariantFieldDefinitions = new List<ProductTypeVariantFieldDefintion>();
        }

        #region Custom field collection callbacks

        private void OnCustomFieldAdded(CustomFieldDefinition field)
        {
            InternalCustomFieldDefinitions.Add(new ProductTypeCustomFieldDefinition
            {
                CustomField = field,
                Sequence = InternalCustomFieldDefinitions.Count == 0 ? 0 : InternalCustomFieldDefinitions.Max(f => f.Sequence) + 1
            });
        }

        private void OnCustomFieldRemoved(CustomFieldDefinition field)
        {
            var internalField = InternalCustomFieldDefinitions.FirstOrDefault(f => f.CustomField.Name == field.Name);
            if (internalField != null)
            {
                InternalCustomFieldDefinitions.Remove(internalField);
            }
        }

        private void OnCustomFieldsSorted()
        {
            var sequence = 0;
            foreach (var field in CustomFieldDefinitions)
            {
                var internalField = InternalCustomFieldDefinitions.FirstOrDefault(f => f.CustomField.Name == field.Name);
                internalField.Sequence = sequence;
                sequence++;
            }
        }

        #endregion

        #region Variant field collection callbacks

        private void OnVariantFieldAdded(CustomFieldDefinition field)
        {
            InternalVariantFieldDefinitions.Add(new ProductTypeVariantFieldDefintion
            {
                CustomField = field,
                Sequence = InternalVariantFieldDefinitions.Count == 0 ? 0 : InternalVariantFieldDefinitions.Max(f => f.Sequence) + 1
            });
        }

        private void OnVariantFieldRemoved(CustomFieldDefinition field)
        {
            var internalField = InternalVariantFieldDefinitions.FirstOrDefault(f => f.CustomField.Name == field.Name);
            if (internalField != null)
            {
                InternalVariantFieldDefinitions.Remove(internalField);
            }
        }

        private void OnVariantFieldsSorted()
        {
            var sequence = 0;
            foreach (var field in VariantFieldDefinitions)
            {
                var internalField = InternalVariantFieldDefinitions.FirstOrDefault(f => f.CustomField.Name == field.Name);
                internalField.Sequence = sequence;
                sequence++;
            }
        }

        #endregion

        class ProductTypeConfiguration : EntityTypeConfiguration<ProductType>
        {
            public ProductTypeConfiguration()
            {
                HasMany(c => c.InternalCustomFieldDefinitions).WithRequired(c => c.ProductType);
                HasMany(c => c.InternalVariantFieldDefinitions).WithRequired(c => c.ProductType);
            }
        }
    }
}
