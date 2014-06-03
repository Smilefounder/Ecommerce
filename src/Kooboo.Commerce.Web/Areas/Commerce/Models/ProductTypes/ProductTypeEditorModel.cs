using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Areas.Commerce.Models.EAV;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes
{
    public class ProductTypeEditorModel
    {
        public ProductTypeEditorModel()
        {
            this.CustomFields = new List<CustomFieldEditorModel>();
            this.VariationFields = new List<CustomFieldEditorModel>();
            this.SystemFields = new List<CustomField>();
        }

        public ProductTypeEditorModel(ProductType type)
            : this()
        {
            this.Id = type.Id;
            this.Name = type.Name;
            this.SkuAlias = type.SkuAlias;
            this.IsEnabled = type.IsEnabled;
            //
            if (type.CustomFields != null)
            {
                foreach (var item in type.CustomFields)
                {
                    this.CustomFields.Add(new CustomFieldEditorModel(item.CustomField));
                }
            }
            //
            if (type.VariationFields != null)
            {
                foreach (var item in type.VariationFields)
                {
                    this.VariationFields.Add(new CustomFieldEditorModel(item.CustomField));
                }
            }
        }

        public void UpdateTo(ProductType type)
        {
            type.Id = this.Id;
            type.Name = (this.Name ?? string.Empty).Trim();
            type.SkuAlias = (this.SkuAlias ?? string.Empty).Trim();

            //
            if (this.CustomFields != null)
            {
                type.CustomFields = new List<ProductTypeCustomField>();
                foreach (var item in this.CustomFields)
                {
                    var field = new ProductTypeCustomField();
                    field.ProductTypeId = type.Id;
                    field.CustomFieldId = item.Id;
                    field.CustomField = new CustomField();
                    item.UpdateTo(field.CustomField);
                    type.CustomFields.Add(field);
                }
            }
            //
            if (this.VariationFields != null)
            {
                type.VariationFields = new List<ProductTypeVariantField>();
                foreach (var item in this.VariationFields)
                {
                    var field = new ProductTypeVariantField();
                    field.ProductTypeId = type.Id;
                    field.CustomFieldId = item.Id;
                    field.CustomField = new CustomField();
                    item.UpdateTo(field.CustomField);
                    type.VariationFields.Add(field);
                }
            }
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [Description("Stands for \"Stock Keeping Unit\"")]
        public string SkuAlias { get; set; }

        public bool IsEnabled { get; set; }

        public IEnumerable<CustomField> SystemFields { get; set; }

        public List<CustomFieldEditorModel> CustomFields { get; set; }

        public List<CustomFieldEditorModel> VariationFields { get; set; }
    }
}