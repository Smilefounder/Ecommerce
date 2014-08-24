using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class RecommendationEngine
    {
        private IFeatureBuilder _featureBuilder;
        private IFeatureItemRelationReader _featureItemRelationReader;

        public RecommendationEngine(IFeatureBuilder featureBuilder, IFeatureItemRelationReader featureItemRelationReader)
        {
            _featureBuilder = featureBuilder;
            _featureItemRelationReader = featureItemRelationReader;
        }

        public IEnumerable<RecommendedItem> Recommend(string userId, int topN, RecommendationOptions options)
        {
            var result = new Dictionary<string, RecommendedItem>();
            var features = _featureBuilder.BuildUserFeatures(userId);
            foreach (var feature in features)
            {
                var relatedItems = _featureItemRelationReader.GetRelatedItems(feature.Id, topN);
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

                    recommendedItem.Weight += item.Value * feature.Weight;
                    recommendedItem.Reasons.Add(feature.Id, item.Value * feature.Weight);
                }
            }

            return result.Values.OrderBy(x => x.Weight).Take(topN);
        }
    }
}