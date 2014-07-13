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

        public Kooboo.Web.Mvc.Paging.IPagedList Execute(Data.CommerceInstance instance, int pageIndex, int pageSize, object config)
        {
            var db = instance.Database;
            var models = db.GetRepository<Product>()
                           .Query()
                           .OrderByDescending(x => x.Id)
                           .ToPagedList(pageIndex, pageSize)
                           .Transform(p => new ProductModel(p));

            return models;
        }
    }
}