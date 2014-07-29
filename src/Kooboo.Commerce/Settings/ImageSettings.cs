using Kooboo.Commerce.ImageSizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Settings
{
    public class ImageSettings
    {
        public List<ImageSize> Sizes { get; set; }

        public ImageSettings()
        {
            Sizes = new List<ImageSize>();
        }
    }
}
