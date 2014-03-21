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

        [Parameter(Name = "PriceId", DisplayName = "Price ID")]
        public int Id { get; set; }

        [Parameter(DisplayName = "Product ID")]
        public int ProductId { get; set; }

        [Parameter(Name = "PriceName", DisplayName = "Price Name")]
        public string Name { get; set; }

        [Parameter]
        public string Sku { get; set; }

        [Parameter]
        public decimal PurchasePrice { get; set; }

        [Parameter]
        public decimal RetailPrice { get; set; }

        [Parameter]
        public int Stock { get; set; }

        [Parameter]
        public int DeliveryDays { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<ProductPriceVariantValue> VariantValues { get; set; }
    }
}
