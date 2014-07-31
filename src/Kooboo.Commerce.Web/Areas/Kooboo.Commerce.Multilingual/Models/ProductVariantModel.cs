using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    public class ProductVariantModel
    {
        public int Id { get; set; }

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public IList<TranslationPair<CustomFieldModel>> VariantFields { get; set; }

        public ProductVariantModel()
        {
            VariantFields = new List<TranslationPair<CustomFieldModel>>();
        }
    }
}