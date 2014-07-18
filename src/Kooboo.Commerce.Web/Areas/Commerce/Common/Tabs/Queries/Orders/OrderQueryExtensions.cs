using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Tabs.Queries.Orders
{
    static class OrderQueryExtensions
    {
        public static IQueryable<Order> ByKeywords(this IQueryable<Order> query, string keywords)
        {
            if (String.IsNullOrWhiteSpace(keywords))
            {
                return query;
            }

            return query.Where(o => o.Customer.FirstName.Contains(keywords) 
                || o.Customer.LastName.Contains(keywords)
                || o.Customer.Email.Contains(keywords));
        }
    }
}