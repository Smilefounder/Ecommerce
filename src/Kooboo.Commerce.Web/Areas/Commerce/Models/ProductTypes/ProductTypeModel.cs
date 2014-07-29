using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Products;
using Kooboo.Commerce.Web.Framework.UI.Grid2;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes.Grid2;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes
{
    [Grid(Checkable = true, IdProperty = "Id", GridItemType = typeof(ProductTypeGridItem))]
    public class ProductTypeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [LinkColumn("Edit")]
        public string Name { get; set; }

        [GridColumn]
        [Description("Stands for \"Stock Keeping Unit\"")]
        public string SkuAlias { get; set; }

        [BooleanColumn]
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