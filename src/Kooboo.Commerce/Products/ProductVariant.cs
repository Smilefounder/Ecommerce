using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Carts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Commerce.Products
{
    public class ProductVariant : ILocalizable
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
        public decimal Price { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<ProductVariantFieldValue> VariantFields { get; set; }

        public ProductVariant()
        {
            VariantFields = new List<ProductVariantFieldValue>();
            CreatedAtUtc = DateTime.UtcNow;
        }

        public ProductVariant(Product product, string name, string sku)
            : this()
        {
            Product = product;
            Name = name;
            Sku = sku;
        }

        public decimal GetFinalPrice(ShoppingContext context)
        {
            return PriceCalculationContext.GetFinalPrice(ProductId, Id, Price, context);
        }

        public virtual void UpdateFrom(ProductVariant other)
        {
            Name = other.Name;
            Sku = other.Sku;
            Price = other.Price;
            UpdateVariantFields(other.VariantFields);
        }

        public virtual void UpdateVariantFields(IEnumerable<ProductVariantFieldValue> values)
        {
            var newValueList = values.ToList();

            foreach (var value in VariantFields.ToList())
            {
                if (!newValueList.Any(f => f.CustomFieldId == value.CustomFieldId))
                {
                    VariantFields.Remove(value);
                }
            }

            foreach (var fieldValue in newValueList)
            {
                var current = VariantFields.FirstOrDefault(f => f.CustomFieldId == fieldValue.CustomFieldId);
                if (current == null)
                {
                    current = new ProductVariantFieldValue(this, fieldValue.CustomFieldId, fieldValue.FieldValue);
                    VariantFields.Add(current);
                }
                else
                {
                    current.FieldValue = fieldValue.FieldValue;
                }
            }
        }
    }
}
