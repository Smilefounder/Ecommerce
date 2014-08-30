using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Products
{
    public class ProductEditorModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IdName Brand { get; set; }

        public int ProductTypeId { get; set; }

        public IList<IdName> Categories { get; set; }

        public IList<ProductImage> Images { get; set; }

        [JsonConverter(typeof(PreserveDictionaryKeyCaseConverter))]
        public IDictionary<string, string> CustomFields { get; set; }

        public IList<ProductVariantModel> Variants { get; set; }

        public bool IsPublished { get; set; }

        public ProductEditorModel()
        {
            Categories = new List<IdName>();
            Images = new List<ProductImage>();
            Variants = new List<ProductVariantModel>();
            CustomFields = new Dictionary<string, string>();
        }
    }
}