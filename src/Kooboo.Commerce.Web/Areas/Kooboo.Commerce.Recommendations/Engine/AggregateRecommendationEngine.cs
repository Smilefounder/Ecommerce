using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class AggregateRecommendationEngine : IRecommendationEngine
    {
        private List<IRecommendationEngine> _engines;

        public AggregateRecommendationEngine()
        {
            _engines = new List<IRecommendationEngine>();
        }

        public AggregateRecommendationEngine(IEnumerable<IRecommendationEngine> engines)
        {
            _engines = new List<IRecommendationEngine>(engines);
        }

        public void Add(IRecommendationEngine engine)
        {
            _engines.Add(engine);
        }

        public IEnumerable<RecommendedItem> Recommend(string userId, int topN)
        {
            var items = new List<RecommendedItem>();
            foreach (var engine in _engines)
            {
                items.AddRange(engine.Recommend(userId, topN));
            }

            return items.OrderByDescending(it => it.Weight).Take(topN).ToList();
        }
    }
}