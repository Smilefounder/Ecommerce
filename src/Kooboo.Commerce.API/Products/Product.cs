using Kooboo.Commerce.API.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? BrandId { get; set; }

        public int ProductTypeId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAtUtc { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        public ProductType Type { get; set; }

        public Brand Brand { get; set; }

        public ProductCategory[] Categories { get; set; }

        public ProductImage[] Images { get; set; }

        public ProductCustomFieldValue[] CustomFieldValues { get; set; }

        public ProductPrice[] PriceList { get; set; }
    }
}
