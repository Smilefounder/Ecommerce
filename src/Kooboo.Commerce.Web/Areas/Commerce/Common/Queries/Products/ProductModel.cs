using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.Commerce.Web.Framework.Queries;

namespace Kooboo.Commerce.Web.Queries.Products
{
    public class ProductModel : IProductModel
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

        [GridColumn]
        public int Id { get; set; }

        [GridColumn(GridItemColumnType = typeof(LinkedGridItemColumn))]
        public string Name { get; set; }

        [BooleanGridColumn(HeaderText = "Published")]
        public bool IsPublished { get; set; }

        public int ProductTypeId { get; set; }
    }
}