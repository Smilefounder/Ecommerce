using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class CustomerController : CommerceAPIControllerAccessBase<Customer>
    {
        protected override ICommerceQuery<Customer> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Customers.Query();
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
                query = query.ByGender((Kooboo.Commerce.API.Gender)(Convert.ToInt32(qs["gender"])));
            if (!string.IsNullOrEmpty(qs["phone"]))
                query = query.ByPhone(qs["phone"]);
            if (!string.IsNullOrEmpty(qs["city"]))
                query = query.ByCity(qs["city"]);
            if (!string.IsNullOrEmpty(qs["countryId"]))
                query = query.ByCountry(Convert.ToInt32(qs["countryId"]));

            if (qs["LoadWithCountry"] == "true")
                query = query.LoadWithCountry();
            if (qs["LoadWithAddresses"] == "true")
                query = query.LoadWithAddresses();
            if (qs["LoadWithCustomerLoyalty"] == "true")
                query = query.LoadWithCustomerLoyalty();

            return query;
        }

        protected override ICommerceAccess<Customer> GetAccesser()
        {
            return Commerce().Customers;
        }
    }
}
