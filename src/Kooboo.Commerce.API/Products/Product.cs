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
    public class Product
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

        public ProductImage GetImage(string imageSizeName)
        {
            if (Images != null)
            {
                var image = Images.FirstOrDefault(o => o.Size == imageSizeName);
                return image;
            }
            return null;
        }

        public string GetCustomFieldValue(string customFieldName)
        {
            if (CustomFieldValues != null)
            {
                var customField = CustomFieldValues.FirstOrDefault(o => o.CustomField.Name == customFieldName);
                if (customField != null)
                {
                    return string.IsNullOrEmpty(customField.FieldValue) ? customField.FieldText : customField.FieldValue;
                }
            }
            return string.Empty;
        }

        public ProductPrice GetLowestPrice()
        {
            if (PriceList != null)
            {
                var price = PriceList.OrderBy(o => o.RetailPrice).FirstOrDefault();
                return price;
            }
            return null;
        }

        public Tuple<decimal, decimal> GetPriceRange()
        {
            if(PriceList != null)
            {
                var lowestPrice = PriceList.OrderBy(o => o.RetailPrice).First();
                var highestPrice = PriceList.OrderBy(o => o.RetailPrice).Last();
                return new Tuple<decimal, decimal>(lowestPrice.RetailPrice, highestPrice.RetailPrice);
            }
            return null;
        }
    }
}
