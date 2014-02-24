using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Web.Areas.Commerce.Models.EAV;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings {

    public class ProductSettingEditorModel {

        public ProductSettingEditorModel() {
        }

        public ProductSettingEditorModel(ProductSetting productSetting) {
            this.SystemFields = new List<CustomFieldEditorModel>();
            if (productSetting.SystemFields != null) {
                foreach (var item in productSetting.SystemFields) {
                    this.SystemFields.Add(new CustomFieldEditorModel(item));
                }
            }
        }

        public void UpdateTo(ProductSetting productSetting) {
            productSetting.SystemFields = new List<CustomField>();
            if (this.SystemFields != null) {
                foreach (var item in this.SystemFields) {
                    var field = new CustomField(); item.UpdateTo(field);
                    productSetting.SystemFields.Add(field);
                }
            }
        }

        public List<CustomFieldEditorModel> SystemFields {
            get;
            set;
        }

        // Other product settings
    }
}