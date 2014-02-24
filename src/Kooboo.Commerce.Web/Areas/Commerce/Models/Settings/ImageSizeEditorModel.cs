using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.ImageSizes;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings {

    public class ImageSizeEditorModel {

        public ImageSizeEditorModel() {
        }

        public ImageSizeEditorModel(ImageSize imageSize)
            : this() {
            this.Name = imageSize.Name;
            this.Width = imageSize.Width;
            this.Height = imageSize.Height;
            this.IsMultiple = imageSize.IsMultiple;
            this.IsEnabled = imageSize.IsEnabled;
            this.IsSystemDefault = imageSize.IsSystemDefault;
        }

        public void UpdateTo(ImageSize imageSize) {
            imageSize.Name = (this.Name ?? string.Empty).Trim();
            imageSize.Width = this.Width;
            imageSize.Height = this.Height;
            imageSize.IsMultiple = this.IsMultiple;
            imageSize.IsEnabled = this.IsEnabled;
            imageSize.IsSystemDefault = this.IsSystemDefault;
        }

        public string Name {
            get;
            set;
        }

        public int Width {
            get;
            set;
        }

        public int Height {
            get;
            set;
        }

        public bool IsMultiple {
            get;
            set;
        }

        public bool IsEnabled {
            get;
            set;
        }

        public bool IsSystemDefault {
            get;
            set;
        }
    }
}