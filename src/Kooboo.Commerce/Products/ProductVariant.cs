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

        // TODO: Seems this field is never used
        [Param]
        public string Name { get; set; }

        [Param]
        public string Sku { get; set; }

        [Param]
        public decimal Price { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<ProductVariantField> VariantFields { get; set; }

        public ProductVariant()
        {
            VariantFields = new List<ProductVariantField>();
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
            UpdateVariantFields(other.VariantFields.ToDictionary(f => f.FieldName, f => f.FieldValue));
        }

        public virtual void UpdateVariantFields(IDictionary<string, string> values)
        {
            foreach (var value in VariantFields.ToList())
            {
                if (!values.ContainsKey(value.FieldName))
                {
                    VariantFields.Remove(value);
                }
            }

            foreach (var value in values)
            {
                var current = VariantFields.FirstOrDefault(f => f.FieldName == value.Key);
                if (current == null)
                {
                    current = new ProductVariantField(value.Key, value.Value);
                    VariantFields.Add(current);
                }
                else
                {
                    current.FieldValue = value.Value;
                }
            }
        }
    }
}
