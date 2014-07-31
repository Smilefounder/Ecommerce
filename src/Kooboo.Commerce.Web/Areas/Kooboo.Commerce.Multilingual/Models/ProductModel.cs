using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<CustomFieldModel> CustomFields { get; set; }

        public IList<ProductVariantModel> Variants { get; set; }

        public ProductModel()
        {
            CustomFields = new List<CustomFieldModel>();
            Variants = new List<ProductVariantModel>();
        }
    }
}