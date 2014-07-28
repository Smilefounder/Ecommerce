using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    public class ProductVariant
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public decimal FinalPrice { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        
        public ICollection<CustomFieldValue> VariantFields { get; set; }

        public ProductVariant()
        {
            VariantFields = new List<CustomFieldValue>();
        }

        public CustomFieldValue GetVariantField(string name)
        {
            return VariantFields.FirstOrDefault(f => f.FieldName == name);
        }
    }
}
