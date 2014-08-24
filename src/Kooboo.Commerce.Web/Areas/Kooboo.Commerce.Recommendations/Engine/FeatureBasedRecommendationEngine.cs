using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class FeatureBasedRecommendationEngine : IRecommendationEngine
    {
        private IFeatureBuilder _featureBuilder;
        private List<IRelatedItemsReader> _relatedItemsReaders;

        public FeatureBasedRecommendationEngine(IEnumerable<Feature> features, IEnumerable<IRelatedItemsReader> relatedItemsReaders)
            : this(new StaticFeatureBuilder(features), relatedItemsReaders)
        {
        }

        public FeatureBasedRecommendationEngine(IFeatureBuilder featureBuilder, IEnumerable<IRelatedItemsReader> relatedItemsReaders)
        {
            _featureBuilder = featureBuilder;
            _relatedItemsReaders = relatedItemsReaders.ToList();
        }

        public IEnumerable<RecommendedItem> Recommend(string userId, int topN)
        {
            var result = new Dictionary<string, RecommendedItem>();
            var features = _featureBuilder.BuildUserFeatures(userId);
            foreach (var feature in features)
            {
                foreach (var reader in _relatedItemsReaders)
                {
                    var relatedItems = reader.GetRelatedItems(feature.Id, topN);
                    foreach (var item in relatedItems)
                    {
                        RecommendedItem recommendedItem;

                        if (!result.TryGetValue(item.Key, out recommendedItem))
                        {
                            recommendedItem = new RecommendedItem
                            {
                                ItemId = item.Key
                            };
                            result.Add(item.Key, recommendedItem);
                        }

                        var weight = item.Value * feature.Weight;

                        recommendedItem.Weight += weight;
                        recommendedItem.Reasons.Add(feature.Id, weight);
                    }
                }
            }

            return result.Values.OrderByDescending(x => x.Weight).Take(topN);
        }
    }
}