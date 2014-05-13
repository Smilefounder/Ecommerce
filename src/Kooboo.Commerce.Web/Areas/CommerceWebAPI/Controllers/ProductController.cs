using Kooboo.Commerce.API;
using Kooboo.Commerce.API.HAL;
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
    public class ProductController : CommerceAPIControllerAccessBase<Product>
    {
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
        [HalParameterProvider()]
        [HalParameter(Name = "id", ParameterType = typeof(int))]
        [HalParameter(Name = "categoryId", ParameterType = typeof(int))]
        [HalParameter(Name = "name", ParameterType = typeof(string))]
        [HalParameter(Name = "containsName", ParameterType = typeof(string))]
        [HalParameter(Name = "productTypeId", ParameterType = typeof(int))]
        [HalParameter(Name = "brandId", ParameterType = typeof(int))]
        [HalParameter(Name = "published", ParameterType = typeof(bool))]
        [HalParameter(Name = "deleted", ParameterType = typeof(bool))]
        [HalParameter(Name = "customField.id", ParameterType = typeof(int))]
        [HalParameter(Name = "customField.name", ParameterType = typeof(string))]
        [HalParameter(Name = "priceVariant.id", ParameterType = typeof(int))]
        [HalParameter(Name = "priceVariant.name", ParameterType = typeof(string))]
        protected override ICommerceQuery<Product> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Products.Query();
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
            if (!string.IsNullOrEmpty(qs["published"]))
                query = query.IsPublished(Convert.ToBoolean(qs["published"]));
            if (!string.IsNullOrEmpty(qs["deleted"]))
                query = query.IsDeleted(Convert.ToBoolean(qs["deleted"]));

            if (!string.IsNullOrEmpty(qs["customField.id"]) && !string.IsNullOrEmpty(qs["customField.value"]))
                query = query.ByCustomField(Convert.ToInt32(qs["customField.id"]), qs["customField.value"]);
            if (!string.IsNullOrEmpty(qs["customField.name"]) && !string.IsNullOrEmpty(qs["customField.value"]))
                query = query.ByCustomField(qs["customField.name"], qs["customField.value"]);
            if (!string.IsNullOrEmpty(qs["priceVariant.id"]) && !string.IsNullOrEmpty(qs["priceVariant.value"]))
                query = query.ByPriceVariant(Convert.ToInt32(qs["priceVariant.id"]), qs["priceVariant.value"]);
            if (!string.IsNullOrEmpty(qs["priceVariant.name"]) && !string.IsNullOrEmpty(qs["priceVariant.value"]))
                query = query.ByPriceVariant(qs["priceVariant.name"], qs["priceVariant.value"]);

            return BuildLoadWithFromQueryStrings(query, qs);
        }

        protected override ICommerceAccess<Product> GetAccesser()
        {
            return Commerce().Products;
        }

    }
}
