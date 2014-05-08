using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    static class PromotionSpecifications
    {
        public static Expression<Func<Promotion, bool>> AvailableNow(DateTime utcNow)
        {
            return promotion =>
                promotion.IsEnabled
                && (promotion.StartTimeUtc == null || promotion.StartTimeUtc <= utcNow)
                && (promotion.EndTimeUtc == null || promotion.EndTimeUtc > utcNow);
        }
    }
}
