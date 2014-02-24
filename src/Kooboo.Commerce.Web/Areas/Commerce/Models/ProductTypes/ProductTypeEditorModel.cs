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

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes {

    public class ProductTypeEditorModel {

        public ProductTypeEditorModel() {
            this.SystemFields = new List<CustomFieldEditorModel>();
            this.CustomFields = new List<CustomFieldEditorModel>();
            this.VariationFields = new List<CustomFieldEditorModel>();
        }

        public ProductTypeEditorModel(ProductType type)
            : this() {
            this.Id = type.Id;
            this.Name = type.Name;
            this.SkuAlias = type.SkuAlias;
            this.IsEnabled = type.IsEnabled;
            //
            if (type.CustomFields != null) {
                foreach (var item in type.CustomFields) {
                    this.CustomFields.Add(new CustomFieldEditorModel(item));
                }
            }
            //
            if (type.VariationFields != null) {
                foreach (var item in type.VariationFields) {
                    this.VariationFields.Add(new CustomFieldEditorModel(item));
                }
            }
        }

        public void UpdateTo(ProductType type) {
            type.Id = this.Id;
            type.Name = (this.Name ?? string.Empty).Trim();
            type.SkuAlias = (this.SkuAlias ?? string.Empty).Trim();
            type.IsEnabled = this.IsEnabled;
            //
            if (this.CustomFields != null) {
                type.CustomFields = new List<CustomField>();
                foreach (var item in this.CustomFields) {
                    var field = new CustomField();
                    item.UpdateTo(field);
                    type.CustomFields.Add(field);
                }
            }
            //
            if (this.VariationFields != null) {
                type.VariationFields = new List<CustomField>();
                foreach (var item in this.VariationFields) {
                    var field = new CustomField();
                    item.UpdateTo(field);
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

        public List<CustomFieldEditorModel> SystemFields { get; set; }

        public List<CustomFieldEditorModel> CustomFields { get; set; }

        public List<CustomFieldEditorModel> VariationFields { get; set; }
    }
}