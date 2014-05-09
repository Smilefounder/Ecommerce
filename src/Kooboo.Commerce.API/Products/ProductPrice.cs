using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// product price
    /// </summary>
    public class ProductPrice
    {
        /// <summary>
        /// price id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// product id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// product price name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// product sku
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// purchase price
        /// </summary>
        public decimal PurchasePrice { get; set; }
        /// <summary>
        /// sale price in retail
        /// </summary>
        public decimal RetailPrice { get; set; }
        /// <summary>
        /// stock count
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// average delivery days
        /// </summary>
        public int DeliveryDays { get; set; }
        /// <summary>
        /// create time at utc
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }
        /// <summary>
        /// is published
        /// </summary>
        public bool IsPublished { get; set; }
        /// <summary>
        /// publish time at utc
        /// </summary>
        public DateTime? PublishedAtUtc { get; set; }
        /// <summary>
        /// product
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// product variant values
        /// </summary>
        public ProductPriceVariantValue[] VariantValues { get; set; }

        public string GetVariantValue(string variantFieldName)
        {
            if (VariantValues != null)
            {
                var customField = VariantValues.FirstOrDefault(o => o.CustomField.Name == variantFieldName);
                if (customField != null)
                {
                    return string.IsNullOrEmpty(customField.FieldValue) ? customField.FieldText : customField.FieldValue;
                }
            }
            return string.Empty;
        }
    }
}
