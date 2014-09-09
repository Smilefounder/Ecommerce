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
            var items = new Dictionary<string, RecommendedItem>();
            foreach (var engine in _engines)
            {
                foreach (var item in engine.Recommend(userId, topN))
                {
                    RecommendedItem existing;
                    if (items.TryGetValue(item.ItemId, out existing))
                    {
                        if (existing.Weight < item.Weight)
                        {
                            items.Remove(item.ItemId);
                            items.Add(item.ItemId, item);
                        }
                    }
                    else
                    {
                        items.Add(item.ItemId, item);
                    }
                }
            }

            return items.Values.OrderByDescending(it => it.Weight).Take(topN).ToList();
        }
    }
}