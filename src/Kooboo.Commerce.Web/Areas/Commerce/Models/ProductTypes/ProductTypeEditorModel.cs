using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Products;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes
{
    public class ProductTypeEditorModel
    {
        public ProductTypeEditorModel()
        {
            SkuAlias = "SKU";
            CustomFields = new List<CustomFieldEditorModel>();
            VariantFields = new List<CustomFieldEditorModel>();
            PredefinedFields = new List<CustomFieldEditorModel>();
        }

        public ProductTypeEditorModel(ProductType type)
            : this()
        {
            Id = type.Id;
            Name = type.Name;
            SkuAlias = type.SkuAlias;
            IsEnabled = type.IsEnabled;


            foreach (var item in type.CustomFields)
            {
                CustomFields.Add(new CustomFieldEditorModel(item));
            }

            foreach (var item in type.VariantFields)
            {
                VariantFields.Add(new CustomFieldEditorModel(item));
            }
        }

        public void UpdateTo(ProductType type)
        {
            type.Id = this.Id;
            type.Name = (this.Name ?? string.Empty).Trim();
            type.SkuAlias = (this.SkuAlias ?? string.Empty).Trim();

            if (this.CustomFields != null)
            {
                foreach (var item in this.CustomFields.OrderBy(f => f.Sequence))
                {
                    var field = new CustomField();
                    item.UpdateTo(field);
                    type.CustomFields.Add(field);
                }
            }

            if (this.VariantFields != null)
            {
                foreach (var item in this.VariantFields)
                {
                    var field = new CustomField();
                    item.UpdateTo(field);
                    type.VariantFields.Add(field);
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

        public List<CustomFieldEditorModel> PredefinedFields { get; set; }

        public List<CustomFieldEditorModel> CustomFields { get; set; }

        public List<CustomFieldEditorModel> VariantFields { get; set; }
    }
}