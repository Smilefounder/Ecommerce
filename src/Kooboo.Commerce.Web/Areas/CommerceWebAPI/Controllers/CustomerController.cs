using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Countries;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
{
    public class CustomerController : CommerceAPIControllerQueryBase<Customer>
    {
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
        protected override ICommerceQuery<Customer> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Customers as ICustomerQuery;
            if (Request.GetRouteData().Values.Keys.Contains("id"))
                query = query.ById(Convert.ToInt32(Request.GetRouteData().Values["id"]));
            if (!string.IsNullOrEmpty(qs["id"]))
                query = query.ById(Convert.ToInt32(qs["id"]));
            if (!string.IsNullOrEmpty(qs["accountId"]))
                query = query.ByAccountId(qs["accountId"]);
            if (!string.IsNullOrEmpty(qs["firstName"]))
                query = query.ByFirstName(qs["firstName"]);
            if (!string.IsNullOrEmpty(qs["middleName"]))
                query = query.ByMiddleName(qs["middleName"]);
            if (!string.IsNullOrEmpty(qs["lastName"]))
                query = query.ByLastName(qs["lastName"]);
            if (!string.IsNullOrEmpty(qs["email"]))
                query = query.ByEmail(qs["email"]);
            if (!string.IsNullOrEmpty(qs["gender"]))
                query = query.ByGender((Kooboo.Commerce.Api.Gender)(Convert.ToInt32(qs["gender"])));
            if (!string.IsNullOrEmpty(qs["phone"]))
                query = query.ByPhone(qs["phone"]);
            if (!string.IsNullOrEmpty(qs["city"]))
                query = query.ByCity(qs["city"]);
            if (!string.IsNullOrEmpty(qs["countryId"]))
                query = query.ByCountry(Convert.ToInt32(qs["countryId"]));
            if (!string.IsNullOrEmpty(qs["customField.name"]) && !string.IsNullOrEmpty(qs["customField.value"]))
                query = query.ByCustomField(qs["customField.name"], qs["customField.value"]);

            return BuildLoadWithFromQueryStrings(query, qs);
        }

        [HttpPost]
        public int PostAddress(int customerId, [FromBody]Address address)
        {
            Commerce().Customers.AddAddress(customerId, address);
            return address.Id;
        }
    }
}
