using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API;
using Kooboo.Commerce.API.HAL;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
{
    /// <summary>
    /// country controller
    /// </summary>
    public class CountryController : CommerceAPIControllerQueryBase<Country>
    {
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
        [HalParameterProvider()]
        [HalParameter(Name = "id", ParameterType = typeof(int))]
        [HalParameter(Name = "name", ParameterType = typeof(string))]
        [HalParameter(Name = "threeLetterISOCode", ParameterType = typeof(string))]
        [HalParameter(Name = "twoLetterISOCode", ParameterType = typeof(string))]
        [HalParameter(Name = "numericISOCode", ParameterType = typeof(string))]
        protected override ICommerceQuery<Country> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Countries.Query();
            if (Request.GetRouteData().Values.Keys.Contains("id"))
                query = query.ById(Convert.ToInt32(Request.GetRouteData().Values["id"]));
            if (!string.IsNullOrEmpty(qs["id"]))
                query = query.ById(Convert.ToInt32(qs["id"]));
            if (!string.IsNullOrEmpty(qs["name"]))
                query = query.ByName(qs["name"]);
            if (!string.IsNullOrEmpty(qs["threeLetterISOCode"]))
                query = query.ByThreeLetterISOCode(qs["threeLetterISOCode"]);
            if (!string.IsNullOrEmpty(qs["twoLetterISOCode"]))
                query = query.ByTwoLetterISOCode(qs["twoLetterISOCode"]);
            if (!string.IsNullOrEmpty(qs["numericISOCode"]))
                query = query.ByNumericISOCode(qs["numericISOCode"]);

            return BuildLoadWithFromQueryStrings(query, qs);
        }
    }
}
