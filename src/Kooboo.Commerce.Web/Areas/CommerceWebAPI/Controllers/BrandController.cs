using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.HAL;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
{
    /// <summary>
    /// brand controller
    /// </summary>
    public class BrandController : CommerceAPIControllerQueryBase<Brand>
    {
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
        [HalParameterProvider()]
        [HalParameter(Name = "id", ParameterType = typeof(int))]
        [HalParameter(Name = "name", ParameterType = typeof(string))]
        [HalParameter(Name = "customField.name", ParameterType = typeof(string))]
        [HalParameter(Name = "customField.value", ParameterType = typeof(string))]
        protected override ICommerceQuery<Brand> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Brands.Query();
            if (Request.GetRouteData().Values.Keys.Contains("id"))
                query = query.ById(Convert.ToInt32(Request.GetRouteData().Values["id"]));
            if (!string.IsNullOrEmpty(qs["id"]))
                query = query.ById(Convert.ToInt32(qs["id"]));
            if (!string.IsNullOrEmpty(qs["name"]))
                query = query.ByName(qs["name"]);
            if (!string.IsNullOrEmpty(qs["customField.name"]) && !string.IsNullOrEmpty(qs["customField.value"]))
                query = query.ByCustomField(qs["customField.name"], qs["customField.value"]);

            return BuildLoadWithFromQueryStrings(query, qs);
        }
    }
}
