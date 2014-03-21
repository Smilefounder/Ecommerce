using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Brands;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class BrandController : CommerceAPIControllerQueryBase<Brand>
    {
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
