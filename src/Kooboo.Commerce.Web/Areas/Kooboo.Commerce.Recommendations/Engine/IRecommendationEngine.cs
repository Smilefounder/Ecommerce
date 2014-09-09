using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public interface IRecommendationEngine
    {
        IEnumerable<RecommendedItem> Recommend(string userId, int topN, ISet<string> ignoredItems);
    }
}