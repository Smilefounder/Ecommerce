using Kooboo.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Customers
{
    static class CustomerQueryExtensions
    {
        public static IQueryable<Customer> ByKeywords(this IQueryable<Customer> query, string keywords)
        {
            if (String.IsNullOrWhiteSpace(keywords))
            {
                return query;
            }

            return query.Where(c => c.FirstName.Contains(keywords) || c.LastName.Contains(keywords) || c.Email.Contains(keywords));
        }
    }
}