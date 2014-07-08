using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Commerce.Products
{
    public class ProductPrice : ILocalizable
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

        public decimal GetFinalPrice(ShoppingContext context)
        {
            return PriceCalculationContext.GetFinalUnitPrice(ProductId, Id, RetailPrice, context);
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
    }
}
