using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class WeightedRelatedItemsProvider : IRelatedItemsProvider
    {
        private IRelatedItemsProvider _provider;

        public IRelatedItemsProvider UnderlyingProvider
        {
            get
            {
                return _provider;
            }
        }

        public float Weight { get; set; }

        public WeightedRelatedItemsProvider(IRelatedItemsProvider provider, float weight)
        {
            _provider = provider;
            Weight = weight;
        }

        public IDictionary<string, double> GetRelatedItems(string featureId, int topN, ISet<string> ignoredItems)
        {
            var relatedItems = _provider.GetRelatedItems(featureId, topN, ignoredItems);
            foreach (var key in relatedItems.Keys.ToList())
            {
                relatedItems[key] *= Weight;
            }

            return relatedItems;
        }
    }
}