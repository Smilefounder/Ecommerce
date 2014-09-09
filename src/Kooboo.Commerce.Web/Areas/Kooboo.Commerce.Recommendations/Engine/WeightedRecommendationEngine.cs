using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class WeightedRecommendationEngine : IRecommendationEngine
    {
        private IRecommendationEngine _engine;

        public float Weight { get; private set; }

        public WeightedRecommendationEngine(IRecommendationEngine engine, float weight)
        {
            _engine = engine;
            Weight = weight;
        }

        public IEnumerable<RecommendedItem> Recommend(string userId, int topN, ISet<string> ignoredItems)
        {
            var items = _engine.Recommend(userId, topN, ignoredItems).ToList();
            foreach (var item in items)
            {
                item.Weight *= Weight;
            }

            return items;
        }
    }
}