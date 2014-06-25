using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.EAV;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// Define the type of a product, the type defines all the custom properties and variations. 
    /// </summary>
    public class ProductType
    {
        /// <summary>
        /// product type id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// product type name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// sku alias
        /// </summary>
        public string SkuAlias { get; set; }
        /// <summary>
        /// is enabled
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// custom fields of this product type
        /// custom field is an extended property to a product, for customize product properties.
        /// the custom field can be selected by user, but it won't affect the product's price.
        /// for example:
        /// user can select color of a mobile phone, but color is not a factor of price.
        /// </summary>
        public CustomField[] CustomFields { get; set; }
        /// <summary>
        /// variant fields of this product type
        /// variant fields are those custom fields that might affect product price.
        /// each price contains a set of values of a product.
        /// for example:
        /// as for product iphon4, there are two prices:
        /// 1. price: $350, with 350mh battery, 4G memory
        /// 2. price: $500, with 500mh battery, 8G memory
        /// user get various price by selecting different set of variants
        /// </summary>
        public CustomField[] VariationFields { get; set; }
    }
}
