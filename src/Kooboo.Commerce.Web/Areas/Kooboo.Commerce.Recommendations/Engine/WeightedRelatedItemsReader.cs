using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class WeightedRelatedItemsReader : IRelatedItemsReader
    {
        private IRelatedItemsReader _reader;

        public float Weight { get; private set; }

        public WeightedRelatedItemsReader(IRelatedItemsReader reader, float weight)
        {
            _reader = reader;
            Weight = weight;
        }

        public IDictionary<string, double> GetRelatedItems(string featureId, int topN)
        {
            var relatedItems = _reader.GetRelatedItems(featureId, topN);
            foreach (var key in relatedItems.Keys)
            {
                relatedItems[key] *= Weight;
            }

            return relatedItems;
        }
    }
}