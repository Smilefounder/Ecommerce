using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.ImageSizes;

namespace Kooboo.Commerce.Settings {

    public class ImageSetting {

        public static ImageSetting NewDefault() {
            return new ImageSetting() {
                Thumbnail = new ImageSize() { IsSystemDefault = true, IsEnabled = true, Width = 240, Height = 240, Name = "Thumbnail" },
                Detail = new ImageSize() { IsSystemDefault = true, IsEnabled = true, Width = 300, Height = 300, Name = "Detail" },
                List = new ImageSize() { IsSystemDefault = true, IsEnabled = true, Width = 35, Height = 35, Name = "List" },
                Cart = new ImageSize() { IsSystemDefault = true, IsEnabled = true, Width = 50, Height = 50, Name = "Cart" },
                CustomSizes = new List<ImageSize>()
            };
        }

        public ImageSize Thumbnail {
            get;
            set;
        }

        public ImageSize Detail {
            get;
            set;
        }

        public ImageSize List {
            get;
            set;
        }

        public ImageSize Cart {
            get;
            set;
        }

        public List<ImageSize> CustomSizes {
            get;
            set;
        }
    }
}
