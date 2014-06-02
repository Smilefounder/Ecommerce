using Kooboo.Commerce.ComponentModel;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    public class ProductPrice
    {
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

        [Obsolete]
        public int Stock { get; set; }

        [Obsolete]
        public int DeliveryDays { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool IsPublished { get; protected set; }

        public DateTime? PublishedAtUtc { get; protected set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<ProductPriceVariantValue> VariantValues { get; set; }

        public ProductPrice()
        {
            VariantValues = new List<ProductPriceVariantValue>();
            CreatedAtUtc = DateTime.UtcNow;
        }

        public ProductPrice(Product product, string name, string sku)
            : this()
        {
            Product = product;
            Name = name;
            Sku = sku;
        }

        public virtual void UpdateFrom(ProductPrice other)
        {
            Name = other.Name;
            Sku = other.Sku;
            PurchasePrice = other.PurchasePrice;
            RetailPrice = other.RetailPrice;
        }

        public virtual void NotifyUpdated()
        {
            Event.Raise(new ProductPriceUpdated(Product, this));
        }

        public virtual bool MarkPublish()
        {
            if (!IsPublished)
            {
                IsPublished = true;
                PublishedAtUtc = DateTime.UtcNow;
                return true;
            }

            return false;
        }

        public virtual bool MarkUnpublish()
        {
            if (IsPublished)
            {
                IsPublished = false;
                PublishedAtUtc = null;
                return true;
            }

            return false;
        }
    }
}
