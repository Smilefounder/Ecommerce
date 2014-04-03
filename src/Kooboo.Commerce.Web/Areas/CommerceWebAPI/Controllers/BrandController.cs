using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Brands;

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
        protected override ICommerceQuery<Brand> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Brands.Query();
            if (!string.IsNullOrEmpty(qs["id"]))
                query = query.ById(Convert.ToInt32(qs["id"]));
            if (!string.IsNullOrEmpty(qs["name"]))
                query = query.ByName(qs["name"]);

            return query;
        }
    }
}
