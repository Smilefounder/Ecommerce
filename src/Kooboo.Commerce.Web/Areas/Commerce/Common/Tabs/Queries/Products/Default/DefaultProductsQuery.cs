using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Tabs.Queries.Products.Default
{
    public class DefaultProductsQuery : ProductTabQuery<ProductModel>
    {
        public override string Name
        {
            get
            {
                return "Products_Default";
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Default";
            }
        }

        public override Pagination Execute(QueryContext context)
        {
            var db = context.Instance.Database;
            var query = db.GetRepository<Product>()
                          .Query()
                          .ByKeywords(context.Keywords);

            var result = query.OrderByDescending(x => x.Id)
                              .Paginate(context.PageIndex, context.PageSize)
                              .Transform(p => new ProductModel(p));

            return result;
        }
    }
}