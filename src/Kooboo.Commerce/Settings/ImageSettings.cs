using Kooboo.Commerce.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Settings
{
    public class ImageSettings
    {
        public List<ImageType> Types { get; set; }

        public ImageSettings()
        {
            Types = new List<ImageType>();
        }
    }
}
