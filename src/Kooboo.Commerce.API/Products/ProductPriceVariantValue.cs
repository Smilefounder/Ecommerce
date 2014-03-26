using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.EAV;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// price variant values of a product
    /// each price contains a set of values of a product.
    /// for example:
    /// as for product iphon4, there are two prices:
    /// 1. price: $350, with 350mh battery, 4G memory
    /// 2. price: $500, with 500mh battery, 8G memory
    /// user get various price by selecting different set of variants
    /// </summary>
    public class ProductPriceVariantValue
    {
        /// <summary>
        /// product price id
        /// </summary>
        public int ProductPriceId { get; set; }
        /// <summary>
        /// custom field id
        /// </summary>
        public int CustomFieldId { get; set; }
        /// <summary>
        /// field value
        /// used for simple value, such as string, char, int, double...
        /// </summary>
        public string FieldValue { get; set; }
        /// <summary>
        /// field text
        /// used for rich text, such as long content, json, xml, html...
        /// </summary>
        public string FieldText { get; set; }

        public ProductPrice ProductPrice { get; set; }
        public CustomField CustomField { get; set; }
    }
}
