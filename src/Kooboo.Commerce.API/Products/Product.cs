using Kooboo.Commerce.API.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// product
    /// </summary>
    public class Product : ItemResource
    {
        /// <summary>
        /// product id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// product name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// brand id
        /// </summary>
        public int? BrandId { get; set; }
        /// <summary>
        /// product type id
        /// </summary>
        public int ProductTypeId { get; set; }
        /// <summary>
        /// create time at utc
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }
        /// <summary>
        /// product type
        /// </summary>
        public ProductType Type { get; set; }
        /// <summary>
        /// brand
        /// </summary>
        public Brand Brand { get; set; }
        /// <summary>
        /// product catgories
        /// </summary>
        public ProductCategory[] Categories { get; set; }

        /// <summary>
        /// images
        /// </summary>
        public ProductImage[] Images { get; set; }
        /// <summary>
        /// custom field valules
        /// </summary>
        public ProductCustomFieldValue[] CustomFieldValues { get; set; }
        /// <summary>
        /// product price list
        /// </summary>
        public ProductPrice[] PriceList { get; set; }
    }
}
