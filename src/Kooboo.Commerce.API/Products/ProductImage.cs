using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// The real image for each product, different size of image can ge rendered from imagesizeservice.
    /// </summary>
    public class ProductImage
    {
        /// <summary>
        /// image id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// product id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// image size name, which is defined in image sizes
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// image url
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// is visible at ui
        /// </summary>
        public bool IsVisible { get; set; }
    }
}
