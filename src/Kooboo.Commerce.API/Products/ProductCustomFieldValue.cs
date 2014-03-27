using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.EAV;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// product custom field value
    /// custom field is an extended property to a product, for customize product properties.
    /// the custom field can be selected by user, but it won't affect the product's price.
    /// for example:
    /// user can select color of a mobile phone, but color is not a factor of price.
    /// </summary>
    public class ProductCustomFieldValue
    {
        /// <summary>
        /// product id
        /// </summary>
        public int ProductId { get; set; }
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
        /// <summary>
        /// product
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// custom field
        /// </summary>
        public CustomField CustomField { get; set; }
    }
}
