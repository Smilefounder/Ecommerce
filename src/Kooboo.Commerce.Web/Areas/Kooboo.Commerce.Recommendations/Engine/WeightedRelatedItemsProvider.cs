using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class WeightedRelatedItemsProvider : IRelatedItemsProvider
    {
        private IRelatedItemsProvider _provider;

        public float Weight { get; private set; }

        public WeightedRelatedItemsProvider(IRelatedItemsProvider provider, float weight)
        {
            _provider = provider;
            Weight = weight;
        }

        public IDictionary<string, double> GetRelatedItems(string featureId, int topN)
        {
            var relatedItems = _provider.GetRelatedItems(featureId, topN);
            foreach (var key in relatedItems.Keys)
            {
                relatedItems[key] *= Weight;
            }

            return relatedItems;
        }
    }
}