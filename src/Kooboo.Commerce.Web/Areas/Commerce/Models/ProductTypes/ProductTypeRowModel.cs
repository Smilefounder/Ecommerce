using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes.Grid2;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes {

    [Grid(Checkable = true, IdProperty = "Id", GridItemType = typeof(ProductTypeRowModelGridItem))]
    public class ProductTypeRowModel {

        public ProductTypeRowModel() {
        }

        public ProductTypeRowModel(ProductType type) {
            this.Id = type.Id;
            this.Name = type.Name;
            this.SkuAlias = type.SkuAlias;
            this.IsEnabled = type.IsEnabled;
        }

        public int Id { get; set; }

        [EditorLinkedGridColumn]
        public string Name { get; set; }

        [GridColumn]
        public string SkuAlias { get; set; }

        [BooleanGridColumn]
        public bool IsEnabled { get; set; }
    }
}