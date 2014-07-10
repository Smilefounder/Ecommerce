using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Queries.Products
{
    [Grid(Checkable = true, IdProperty = "Id")]
    public class ProductModel
    {
        public ProductModel()
        {
        }

        public ProductModel(Product product)
            : this()
        {
            Id = product.Id;
            Name = product.Name;
            IsPublished = product.IsPublished;
            ProductTypeId = product.ProductTypeId;
        }

        public int Id { get; set; }

        [GridColumn(GridItemColumnType = typeof(LinkedGridItemColumn))]
        public string Name { get; set; }

        [BooleanGridColumn(HeaderText = "Published")]
        public bool IsPublished { get; set; }

        public int ProductTypeId { get; set; }
    }
}