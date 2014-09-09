using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public static class RelatedItemsProviders
    {
        static readonly Dictionary<string, List<WeightedRelatedItemsProvider>> _providersByInstances = new Dictionary<string, List<WeightedRelatedItemsProvider>>();

        public static IEnumerable<WeightedRelatedItemsProvider> GetProviders(string instance)
        {
            return _providersByInstances[instance];
        }

        public static void RemoveProviders(string instance)
        {
            _providersByInstances.Remove(instance);
        }

        public static void Register(string instance, IRelatedItemsProvider provider, float weight = 1f)
        {
            Register(instance, new[] { new WeightedRelatedItemsProvider(provider, weight) });
        }

        public static void Register(string instance, IEnumerable<WeightedRelatedItemsProvider> providers)
        {
            if (!_providersByInstances.ContainsKey(instance))
            {
                _providersByInstances.Add(instance, new List<WeightedRelatedItemsProvider>(providers));
            }
            else
            {
                var collection = _providersByInstances[instance];
                foreach (var provider in providers)
                {
                    collection.Add(provider);
                }
            }
        }
    }
}