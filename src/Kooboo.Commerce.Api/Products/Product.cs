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

        public decimal LowestPrice { get; set; }

        public decimal HighestPrice { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        [OptionalInclude]
        public Brand Brand { get; set; }

        [OptionalInclude]
        public IList<Category> Categories { get; set; }

        [OptionalInclude]
        public IList<ProductImage> Images { get; set; }

        [OptionalInclude]
        public IList<CustomField> CustomFields { get; set; }

        [OptionalInclude]
        public IList<ProductVariant> Variants { get; set; }

        public Product()
        {
            Categories = new List<Category>();
            Images = new List<ProductImage>();
            CustomFields = new List<CustomField>();
            Variants = new List<ProductVariant>();
        }

        public ProductImage GetImage(string type)
        {
            if (Images != null)
            {
                return Images.FirstOrDefault(it => it.Type == type);
            }

            return null;
        }

        public string GetImageUrl(string type)
        {
            var image = GetImage(type);
            return image == null ? null : image.ImageUrl;
        }

        public IEnumerable<ProductImage> GetImages(string type)
        {
            if (Images != null)
            {
                return Images.Where(it => it.Type == type);
            }

            return Enumerable.Empty<ProductImage>();
        }

        public IEnumerable<string> GetImageUrls(string type)
        {
            return GetImages(type).Select(it => it.ImageUrl);
        }

        public CustomField GetCustomField(string fieldName)
        {
            if (CustomFields != null)
            {
                return CustomFields.FirstOrDefault(o => o.FieldName == fieldName);
            }

            return null;
        }
    }
}
