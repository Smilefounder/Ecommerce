using Kooboo.Commerce.Web.Framework.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Products
{
    public class ProductVariantModel
    {
        public int Id { get; set; }

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public bool IsEditing { get; set; }

        public bool Selected { get; set; }

        [JsonConverter(typeof(PreserveDictionaryKeyCaseConverter))]
        public IDictionary<string, string> VariantFields { get; set; }

        public ProductVariantModel()
        {
            VariantFields = new Dictionary<string, string>();
        }
    }
}