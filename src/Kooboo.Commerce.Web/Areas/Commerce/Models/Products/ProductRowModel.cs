using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Products {

    [Grid(Checkable = true, IdProperty = "Id")]
    public class ProductRowModel {

        public ProductRowModel() {
        }

        public ProductRowModel(Product product)
            : this() {
            this.Id = product.Id;
            this.Name = product.Name;
            this.IsPublished = product.IsPublished;
            this.ProductTypeId = product.ProductTypeId;
        }

        public int Id { get; set; }

        [GridColumn(GridItemColumnType = typeof(EditGridActionItemColumn))]
        public string Name { get; set; }

        [BooleanGridColumn(HeaderText = "Published")]
        public bool IsPublished { get; set; }

        public int ProductTypeId { get; set; }
    }
}