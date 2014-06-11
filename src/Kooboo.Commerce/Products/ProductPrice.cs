using Kooboo.Commerce.ComponentModel;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.ShoppingCarts;
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

        public decimal GetFinalRetialPrice(ShoppingContext context)
        {
            return PricingContext.GetFinalRetailPrice(ProductId, Id, RetailPrice, context);
        }

        public virtual void UpdateFrom(ProductPrice other)
        {
            Name = other.Name;
            Sku = other.Sku;
            PurchasePrice = other.PurchasePrice;
            RetailPrice = other.RetailPrice;
            UpdateVariantValues(other.VariantValues);
        }

        public virtual void UpdateVariantValues(IEnumerable<ProductPriceVariantValue> values)
        {
            var newValueList = values.ToList();

            foreach (var value in VariantValues.ToList())
            {
                if (!newValueList.Any(f => f.CustomFieldId == value.CustomFieldId))
                {
                    VariantValues.Remove(value);
                }
            }

            foreach (var fieldValue in newValueList)
            {
                var current = VariantValues.FirstOrDefault(f => f.CustomFieldId == fieldValue.CustomFieldId);
                if (current == null)
                {
                    current = new ProductPriceVariantValue(this, fieldValue.CustomFieldId, fieldValue.FieldValue);
                    VariantValues.Add(current);
                }
                else
                {
                    current.FieldValue = fieldValue.FieldValue;
                }
            }
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
