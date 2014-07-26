using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.Api.Brands;
using Kooboo.Commerce.Api.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SkuAlias { get; set; }

        public int? BrandId { get; set; }

        public int ProductTypeId { get; set; }

        public PriceRange PriceRange { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public Brand Brand { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<ProductImage> Images { get; set; }

        public ICollection<CustomFieldValue> CustomFields { get; set; }

        public ICollection<ProductVariant> Variants { get; set; }

        public Product()
        {
            Categories = new List<Category>();
            Images = new List<ProductImage>();
            CustomFields = new List<CustomFieldValue>();
            Variants = new List<ProductVariant>();
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
    }
}
