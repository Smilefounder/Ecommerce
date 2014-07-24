using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Products;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
{
    public class ProductController : CommerceAPIControllerQueryBase<Product>
    {
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
        protected override ICommerceQuery<Product> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Products as IProductQuery;
            if (Request.GetRouteData().Values.Keys.Contains("id"))
                query = query.ById(Convert.ToInt32(Request.GetRouteData().Values["id"]));
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
            if (!string.IsNullOrEmpty(qs["customField.name"]) && !string.IsNullOrEmpty(qs["customField.value"]))
                query = query.ByCustomField(qs["customField.name"], qs["customField.value"]);
            if (!string.IsNullOrEmpty(qs["priceVariant.name"]) && !string.IsNullOrEmpty(qs["priceVariant.value"]))
                query = query.ByPriceVariant(qs["priceVariant.name"], qs["priceVariant.value"]);

            return BuildLoadWithFromQueryStrings(query, qs);
        }
    }
}
