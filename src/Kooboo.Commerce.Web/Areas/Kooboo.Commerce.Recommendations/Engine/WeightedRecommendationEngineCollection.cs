using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class WeightedRecommendationEngineCollection : Collection<WeightedRecommendationEngine>, IRecommendationEngine
    {
        public WeightedRecommendationEngineCollection()
        {
        }

        public WeightedRecommendationEngineCollection(IEnumerable<WeightedRecommendationEngine> engines)
            : base(engines.ToList())
        {
        }

        public void Add(IRecommendationEngine engine, float weight)
        {
            Add(new WeightedRecommendationEngine(engine, weight));
        }

        public IEnumerable<RecommendedItem> Recommend(string userId, int topN, ISet<string> ignoredItems)
        {
            var items = new Dictionary<string, RecommendedItem>();

            foreach (var engine in Items)
            {
                foreach (var item in engine.Recommend(userId, topN, ignoredItems))
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