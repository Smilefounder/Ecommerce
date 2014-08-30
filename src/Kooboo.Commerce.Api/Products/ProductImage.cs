using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.Commerce.Api.Products
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string ImageUrl { get; set; }
    }
}
