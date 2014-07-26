using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    /// <summary>
    /// product
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SkuAlias { get; set; }

        public int? BrandId { get; set; }

        public int ProductTypeId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public Brand Brand { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<ProductImage> Images { get; set; }

        public ICollection<CustomFieldValue> CustomFields { get; set; }

        public ProductPrice[] PriceList { get; set; }

        public Product()
        {
            Categories = new List<Category>();
            Images = new List<ProductImage>();
            CustomFields = new List<CustomFieldValue>();
        }

        public ProductImage GetImage(string imageSizeName)
        {
            if (Images != null)
            {
                var image = Images.FirstOrDefault(o => o.Size == imageSizeName);
                return image;
            }
            return null;
        }

        public CustomFieldValue GetCustomField(string fieldName)
        {
            if (CustomFields != null)
            {
                return CustomFields.FirstOrDefault(o => o.FieldName == fieldName);
            }

            return null;
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
