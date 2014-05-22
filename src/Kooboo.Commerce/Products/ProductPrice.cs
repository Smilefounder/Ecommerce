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

        [Param]
        public int Id { get; set; }

        [Param]
        public int ProductId { get; set; }

        [Param]
        public string Name { get; set; }

        [Param]
        public string Sku { get; set; }

        [Param]
        public decimal PurchasePrice { get; set; }

        [Param]
        public decimal RetailPrice { get; set; }

        [Param]
        public int Stock { get; set; }

        [Param]
        public int DeliveryDays { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<ProductPriceVariantValue> VariantValues { get; set; }
    }
}
