using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Categories;
using Kooboo.Commerce.API.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
{
    public class CategoryController : CommerceAPIControllerQueryBase<Category>
    {
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
        [HalParameterProvider()]
        [HalParameter(Name = "id", ParameterType = typeof(int))]
        [HalParameter(Name = "name", ParameterType = typeof(string))]
        [HalParameter(Name = "published", ParameterType = typeof(bool))]
        [HalParameter(Name = "parentId", ParameterType = typeof(int))]
        [HalParameter(Name = "customField.name", ParameterType = typeof(string))]
        [HalParameter(Name = "customField.value", ParameterType = typeof(string))]
        protected override ICommerceQuery<Category> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Categories.Query();
            if (Request.GetRouteData().Values.Keys.Contains("id"))
                query = query.ById(Convert.ToInt32(Request.GetRouteData().Values["id"]));
            if (!string.IsNullOrEmpty(qs["id"]))
                query = query.ById(Convert.ToInt32(qs["id"]));
            if (!string.IsNullOrEmpty(qs["name"]))
                query = query.ByName(qs["name"]);
            if (!string.IsNullOrEmpty(qs["published"]))
                query = query.Published(Convert.ToBoolean(qs["published"]));
            if (qs["parentId"] != null)
            {
                if (!string.IsNullOrEmpty(qs["parentId"]))
                    query = query.ByParentId(Convert.ToInt32(qs["parentId"]));
                else
                    query = query.ByParentId(null);
            }
            if (!string.IsNullOrEmpty(qs["customField.name"]) && !string.IsNullOrEmpty(qs["customField.value"]))
                query = query.ByCustomField(qs["customField.name"], qs["customField.value"]);

            return BuildLoadWithFromQueryStrings(query, qs);
        }

    }
}
