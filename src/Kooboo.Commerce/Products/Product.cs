using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;

namespace Kooboo.Commerce.Products
{
    /// <summary>
    /// This class store the product information, but the sale items. The items to be sold stored in the ProductVariant. 
    /// One product can have multiple variants, each variant contains some field values and some price, stock, etc. 
    /// </summary>
    public class Product
    {
        public Product()
        {
            CreatedAtUtc = DateTime.UtcNow;
            IsDeleted = false;
            IsPublished = false;
            Type = null;
            Brand = null;
            Categories = new List<ProductCategory>();
            Images = new List<ProductImage>();
            CustomFieldValues = new List<ProductCustomFieldValue>();
            PriceList = new List<ProductPrice>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? BrandId { get; set; }

        public int ProductTypeId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAtUtc { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        public virtual ProductType Type { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual ICollection<ProductCategory> Categories { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; }

        public virtual ICollection<ProductCustomFieldValue> CustomFieldValues { get; set; }

        public virtual ICollection<ProductPrice> PriceList { get; set; }
    }
}