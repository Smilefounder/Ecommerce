using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public static class RelatedItemsProviders
    {
        static readonly Dictionary<string, List<IRelatedItemsProvider>> _providersByInstances = new Dictionary<string,List<IRelatedItemsProvider>>();

        public static IEnumerable<IRelatedItemsProvider> GetProviders(string instance)
        {
            return _providersByInstances[instance];
        }

        public static void RemoveProviders(string instance)
        {
            _providersByInstances.Remove(instance);
        }

        public static void AddProvider(string instance, IRelatedItemsProvider provider)
        {
            AddProviders(instance, new[] { provider });
        }

        public static void AddProviders(string instance, IEnumerable<IRelatedItemsProvider> providers)
        {
            if (!_providersByInstances.ContainsKey(instance))
            {
                _providersByInstances.Add(instance, providers.ToList());
            }
            else
            {
                _providersByInstances[instance].AddRange(providers);
            }
        }
    }
}