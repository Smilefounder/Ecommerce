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

        public ProductVariant(Product product)
            : this()
        {
            Product = product;
        }

        public decimal GetFinalPrice(ShoppingContext context)
        {
            return PriceCalculationContext.GetFinalPrice(ProductId, Id, Price, context);
        }

        public void SetVariantField(string name, string value)
        {
            var field = VariantFields.FirstOrDefault(f => f.FieldName == name);
            if (field != null)
            {
                field.FieldValue = value;
            }
            else
            {
                field = new ProductVariantField(name, value);
                VariantFields.Add(field);
            }
        }

        public void SetVariantFields(IDictionary<string, string> fieldValues)
        {
            foreach (var field in VariantFields.ToList())
            {
                if (!fieldValues.ContainsKey(field.FieldName))
                {
                    VariantFields.Remove(field);
                }
            }

            foreach (var fieldValue in fieldValues)
            {
                SetVariantField(fieldValue.Key, fieldValue.Value);
            }
        }
    }
}
