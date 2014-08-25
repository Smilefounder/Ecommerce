using Kooboo.Commerce.Recommendations.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.CMSIntegration
{
    static class RecommendedItemExtensions
    {
        public static int GetBestReasonItemId(this RecommendedItem item)
        {
            if (item.Reasons.Count == 0)
            {
                return -1;
            }

            var bestReason = item.Reasons.OrderByDescending(it => it.Value).First();
            var bestReasonItemId = 0;

            if (Int32.TryParse(bestReason.Key, out bestReasonItemId))
            {
                return bestReasonItemId;
            }

            return -1;
        }
    }
}