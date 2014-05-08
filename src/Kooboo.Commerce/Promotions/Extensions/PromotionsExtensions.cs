using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public static class PromotionsExtensions
    {
        public static Promotion ByCoupon(this IQueryable<Promotion> query, string coupon)
        {
            return query.Where(p => p.RequireCouponCode && p.CouponCode == coupon).FirstOrDefault();
        }

        public static IQueryable<Promotion> WhereAvailableNow(this IQueryable<Promotion> query)
        {
            return query.WhereAvailableNow(DateTime.UtcNow);
        }

        public static IQueryable<Promotion> WhereAvailableNow(this IQueryable<Promotion> query, DateTime utcNow)
        {
            return query.Where(PromotionSpecifications.AvailableNow(utcNow));
        }
    }
}
