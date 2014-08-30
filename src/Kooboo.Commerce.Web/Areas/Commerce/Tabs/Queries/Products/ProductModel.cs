using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Products;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using Kooboo.Commerce.Web.Framework.UI.Grid2;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Products
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
            ProductTypeId = product.ProductType.Id;
        }

        [GridColumn]
        public int Id { get; set; }

        [LinkColumn("Edit")]
        public string Name { get; set; }

        [BooleanColumn(HeaderText = "Published")]
        public bool IsPublished { get; set; }

        public int ProductTypeId { get; set; }
    }
}