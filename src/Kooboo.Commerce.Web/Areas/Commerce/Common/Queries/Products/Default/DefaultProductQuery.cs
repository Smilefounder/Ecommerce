using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Queries.Products.Default
{
    public class DefaultProductQuery : IProductQuery
    {
        public string Name
        {
            get
            {
                return "AllProducts";
            }
        }

        public string DisplayName
        {
            get
            {
                return "All";
            }
        }

        public Type ConfigType
        {
            get
            {
                return null;
            }
        }

        public Type ResultType
        {
            get
            {
                return typeof(ProductModel);
            }
        }

        public Pagination Execute(QueryContext context)
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