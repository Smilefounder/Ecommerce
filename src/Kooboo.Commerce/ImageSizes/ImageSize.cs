using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Kooboo.Commerce.Settings;

namespace Kooboo.Commerce.ImageSizes
{
    /// <summary>
    /// predefined the image size that can be generated for each products. 
    /// 
    /// <usage>
    /// Different size of image can be queried like /image/small/imagename.jpg
    /// </usage>
    /// </summary>
    public class ImageSize
    {
        /// <summary>
        /// key
        /// </summary>
        [Key]
        public  string Name { get; set; }

        public  int Width { get; set; }

        public  int Height { get; set; }

        /// <summary>
        /// Type of size can be disable and enabled. Because there are some system default size that will appear alwayws, we this isEnable field to hide system default size.
        /// </summary>
        public  bool IsEnabled { get;set;}

        public bool IsMultiple { get; set; }

        public bool IsSystemDefault { get; set; }
    }
}
