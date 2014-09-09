using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public static class RecommendationEngines
    {
        static readonly Dictionary<string, WeightedRecommendationEngineCollection> _enginesByInstance = new Dictionary<string, WeightedRecommendationEngineCollection>();

        public static WeightedRecommendationEngineCollection GetEngines(string instance)
        {
            return _enginesByInstance[instance];
        }

        public static bool RemoveEngines(string instance)
        {
            return _enginesByInstance.Remove(instance);
        }

        public static void Register(string instance, IRecommendationEngine engine, float weight = 1f)
        {
            if (!_enginesByInstance.ContainsKey(instance))
            {
                _enginesByInstance.Add(instance, new WeightedRecommendationEngineCollection
                {
                    new WeightedRecommendationEngine(engine, weight)
                });
            }
            else
            {
                _enginesByInstance[instance].Add(engine, weight);
            }
        }
    }
}