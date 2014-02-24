using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Kooboo.Commerce.ImageSizes;

namespace Kooboo.Commerce.Data.Mapping
{
    public class ImageSizeMap : EntityTypeConfiguration<ImageSize> {

        public ImageSizeMap() {
            this.HasKey(o => o.Name);
        }
    }
}
