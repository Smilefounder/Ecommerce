using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.ImageSizes;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings {

    public class ImageSettingEditorModel {

        public ImageSettingEditorModel() {
        }

        public ImageSettingEditorModel(ImageSetting imageSetting) {
            if (imageSetting.Thumbnail != null) {
                this.Thumbnail = new ImageSizeEditorModel(imageSetting.Thumbnail);
            }
            if (imageSetting.Detail != null) {
                this.Detail = new ImageSizeEditorModel(imageSetting.Detail);
            }
            if (imageSetting.List != null) {
                this.List = new ImageSizeEditorModel(imageSetting.List);
            }
            if (imageSetting.Cart != null) {
                this.Cart = new ImageSizeEditorModel(imageSetting.Cart);
            }
            //
            if (imageSetting.CustomSizes != null) {
                this.CustomSizes = new List<ImageSizeEditorModel>();
                foreach (var item in imageSetting.CustomSizes) {
                    this.CustomSizes.Add(new ImageSizeEditorModel(item));
                }
            }
        }

        public void UpdateTo(ImageSetting imageSetting) {
            if (this.Thumbnail != null) {
                if (imageSetting.Thumbnail == null) { imageSetting.Thumbnail = new ImageSize(); }
                this.Thumbnail.UpdateTo(imageSetting.Thumbnail);
                imageSetting.Thumbnail.IsSystemDefault = true;
            }
            if (this.Detail != null) {
                if (imageSetting.Detail == null) { imageSetting.Detail = new ImageSize(); }
                this.Detail.UpdateTo(imageSetting.Detail);
                imageSetting.Detail.IsSystemDefault = true;
            }
            if (this.List != null) {
                if (imageSetting.List == null) { imageSetting.List = new ImageSize(); }
                this.List.UpdateTo(imageSetting.List);
                imageSetting.List.IsSystemDefault = true;
            }
            if (this.Cart != null) {
                if (imageSetting.Cart == null) { imageSetting.Cart = new ImageSize(); }
                this.Cart.UpdateTo(imageSetting.Cart);
                imageSetting.Cart.IsSystemDefault = true;
            }
            //
            if (this.CustomSizes != null) {
                imageSetting.CustomSizes = new List<ImageSize>();
                foreach (var item in this.CustomSizes) {
                    var size = new ImageSize();
                    item.UpdateTo(size);
                    imageSetting.CustomSizes.Add(size);
                }
            }
        }

        public ImageSizeEditorModel Thumbnail {
            get;
            set;
        }

        public ImageSizeEditorModel Detail {
            get;
            set;
        }

        public ImageSizeEditorModel List {
            get;
            set;
        }

        public ImageSizeEditorModel Cart {
            get;
            set;
        }

        public List<ImageSizeEditorModel> CustomSizes {
            get;
            set;
        }
    }
}