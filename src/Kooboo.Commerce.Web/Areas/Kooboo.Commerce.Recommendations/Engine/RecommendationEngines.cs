using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public static class RecommendationEngines
    {
        static readonly Dictionary<string, IRecommendationEngine> _enginesByInstance = new Dictionary<string, IRecommendationEngine>();

        public static IRecommendationEngine Get(string instance)
        {
            return _enginesByInstance[instance];
        }

        public static bool Remove(string instance)
        {
            return _enginesByInstance.Remove(instance);
        }

        public static void Set(string instance, IRecommendationEngine engine)
        {
            if (!_enginesByInstance.ContainsKey(instance))
            {
                _enginesByInstance.Add(instance, engine);
            }
            else
            {
                _enginesByInstance[instance] = engine;
            }
        }
    }
}