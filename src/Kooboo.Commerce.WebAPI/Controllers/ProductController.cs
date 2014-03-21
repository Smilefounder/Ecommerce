using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Products;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class ProductController : CommerceAPIControllerAccessBase<Product>
    {
        protected override ICommerceQuery<Product> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Products.Query();
            if (!string.IsNullOrEmpty(qs["id"]))
                query = query.ById(Convert.ToInt32(qs["id"]));
            if (!string.IsNullOrEmpty(qs["categoryId"]))
                query = query.ByCategoryId(Convert.ToInt32(qs["categoryId"]));
            if (!string.IsNullOrEmpty(qs["name"]))
                query = query.ByName(qs["name"]);
            if (!string.IsNullOrEmpty(qs["containsName"]))
                query = query.ContainsName(qs["containsName"]);
            if (!string.IsNullOrEmpty(qs["productTypeId"]))
                query = query.ByProductTypeId(Convert.ToInt32(qs["productTypeId"]));
            if (!string.IsNullOrEmpty(qs["brandId"]))
                query = query.ByBrandId(Convert.ToInt32(qs["brandId"]));
            if (!string.IsNullOrEmpty(qs["published"]))
                query = query.IsPublished(Convert.ToBoolean(qs["published"]));
            if (!string.IsNullOrEmpty(qs["deleted"]))
                query = query.IsDeleted(Convert.ToBoolean(qs["deleted"]));

            if (qs["LoadWithProductType"] == "true")
                query = query.LoadWithProductType();
            if (qs["LoadWithBrand"] == "true")
                query = query.LoadWithBrand();
            if (qs["LoadWithCategories"] == "true")
                query = query.LoadWithCategories();
            if (qs["LoadWithImages"] == "true")
                query = query.LoadWithImages();
            if (qs["LoadWithCustomFields"] == "true")
                query = query.LoadWithCustomFields();
            if (qs["LoadWithPriceList"] == "true")
                query = query.LoadWithPriceList();

            return query;
        }

        protected override ICommerceAccess<Product> GetAccesser()
        {
            return Commerce().Products;
        }

    }
}
