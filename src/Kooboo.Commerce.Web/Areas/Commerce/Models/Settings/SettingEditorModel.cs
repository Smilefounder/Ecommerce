using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Settings;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings {

    public class SettingEditorModel {

        public SettingEditorModel() {
        }

        public SettingEditorModel(StoreSetting storeSetting, ImageSetting imageSetting, ProductSetting productSetting) {
            this.StoreSetting = new StoreSettingEditorModel(storeSetting);
            this.ImageSetting = new ImageSettingEditorModel(imageSetting);
            this.ProductSetting = new ProductSettingEditorModel(productSetting);
        }

        public StoreSettingEditorModel StoreSetting {
            get;
            set;
        }

        public ImageSettingEditorModel ImageSetting {
            get;
            set;
        }

        public ProductSettingEditorModel ProductSetting {
            get;
            set;
        }
    }
}