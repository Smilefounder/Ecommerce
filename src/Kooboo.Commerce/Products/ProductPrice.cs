using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    public class ProductPrice
    {
        public ProductPrice()
        {
            Product = null;
            VariantValues = new List<ProductPriceVariantValue>();
        }

        [ConditionParameter(Name = "PriceId", DisplayName = "Price ID")]
        public int Id { get; set; }

        [ConditionParameter(DisplayName = "Product ID")]
        public int ProductId { get; set; }

        [ConditionParameter(Name = "PriceName", DisplayName = "Price Name")]
        public string Name { get; set; }

        [ConditionParameter]
        public string Sku { get; set; }

        [ConditionParameter]
        public decimal PurchasePrice { get; set; }

        [ConditionParameter]
        public decimal RetailPrice { get; set; }

        [ConditionParameter]
        public int Stock { get; set; }

        [ConditionParameter]
        public int DeliveryDays { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<ProductPriceVariantValue> VariantValues { get; set; }
    }
}
