using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public static class PromotionsExtensions
    {
        public static IQueryable<Promotion> WhereAvailableNow(this IQueryable<Promotion> query)
        {
            return query.WhereAvailableNow(DateTime.UtcNow);
        }

        public static IQueryable<Promotion> WhereAvailableNow(this IQueryable<Promotion> query, DateTime utcNow)
        {
            return query.Where(x => x.IsEnabled)
                        .Where(x => x.StartTimeUtc == null || x.StartTimeUtc <= utcNow)
                        .Where(x => x.EndTimeUtc == null || x.EndTimeUtc > utcNow);
        }
    }
}
