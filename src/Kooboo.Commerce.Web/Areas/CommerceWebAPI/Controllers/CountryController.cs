using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.Api.Countries;
using Kooboo.Commerce.Api;

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
        protected override ICommerceQuery<Country> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Countries as ICountryQuery;
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
