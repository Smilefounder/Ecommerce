using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes.Grid2;
using Kooboo.Commerce.Web.Framework.UI.Grid2;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes
{

    [Grid(Checkable = true, IdProperty = "Id", GridItemType = typeof(ProductTypeRowModelGridItem))]
    public class ProductTypeRowModel
    {

        public ProductTypeRowModel()
        {
        }

        public ProductTypeRowModel(ProductType type)
        {
            Id = type.Id;
            Name = type.Name;
            SkuAlias = type.SkuAlias;
            IsEnabled = type.IsEnabled;
        }

        public int Id { get; set; }

        [LinkColumn("Edit")]
        public string Name { get; set; }

        [GridColumn]
        public string SkuAlias { get; set; }

        [BooleanColumn]
        public bool IsEnabled { get; set; }
    }
}