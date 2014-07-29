using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Products;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes
{
    public class ProductTypeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Description("Stands for \"Stock Keeping Unit\"")]
        public string SkuAlias { get; set; }

        public bool IsEnabled { get; set; }

        public List<CustomField> PredefinedFields { get; set; }

        public List<CustomField> CustomFields { get; set; }

        public List<CustomField> VariantFields { get; set; }

        public ProductTypeModel()
        {
            CustomFields = new List<CustomField>();
            VariantFields = new List<CustomField>();
        }
    }
}