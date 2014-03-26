using Kooboo.Commerce.API.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// product category
    /// </summary>
    public class ProductCategory
    {
        /// <summary>
        /// product id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// category id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// product
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// category
        /// </summary>
        public Category Category { get; set; }
    }
}
