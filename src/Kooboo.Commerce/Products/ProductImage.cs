using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    /// <summary>
    /// The real image for each product, different size of image can ge rendered from imagesizeservice.
    /// </summary>
    public class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ImageSizeName { get; set; }

        public string ImageUrl { get; set; }

        public bool IsVisible { get; set; }

        public Product Product { get; set; }
    }
}
