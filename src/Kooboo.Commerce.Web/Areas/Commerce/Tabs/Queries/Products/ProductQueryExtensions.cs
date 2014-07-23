using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Products
{
    static class ProductQueryExtensions
    {
        public static IQueryable<Product> ByKeywords(this IQueryable<Product> query, string keywords)
        {
            if (String.IsNullOrWhiteSpace(keywords))
            {
                return query;
            }

            return query.Where(p => p.Name.Contains(keywords));
        }
    }
}