using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    public class ProductPrice
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal PurchasePrice { get; set; }

        public decimal RetailPrice { get; set; }

        public int Stock { get; set; }

        public int DeliveryDays { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        public Product Product { get; set; }

        public ProductPriceVariantValue[] VariantValues { get; set; }
    }
}
