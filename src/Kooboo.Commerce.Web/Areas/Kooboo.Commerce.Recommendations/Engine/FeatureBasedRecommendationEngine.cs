using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    /// <summary>
    /// 基于特征的推荐引擎。
    /// </summary>
    /// <remarks>
    /// 它将用户和物品通过用户的特征来关联，推荐流程为：
    /// 
    /// (1) 构建当前用户的特征（特征ID及权重）。
    ///     用户的行为可以认为是用户的一种特征，例如用户购买了 iPhone，则 iPhone 可以作为该用户的特征，权重可以为1。
    ///     用户的人口统计学属性也可以是用户的特征，例如用户是男性，是中国人等。
    ///     
    /// (2) 获取每个特征关联的物品及权重。
    ///     每个特征都关联了一定数量的物品及权重（事先算好），例如把用户购买的 iPhone 作为用户特征时，
    ///     iPhone 是特征ID，它关联的物品及为相似度矩阵中与其最相似的物品。
    /// 
    /// (3) 将特征权重及其相关联物品的权重相乘，即为推荐物品的权重;
    /// </remarks>
    public class FeatureBasedRecommendationEngine : IRecommendationEngine
    {
        private IFeatureBuilder _featureBuilder;
        private List<IRelatedItemsProvider> _relatedItemsReaders;

        public FeatureBasedRecommendationEngine(IEnumerable<Feature> features, IEnumerable<IRelatedItemsProvider> relatedItemsReaders)
            : this(new StaticFeatureBuilder(features), relatedItemsReaders)
        {
        }

        public FeatureBasedRecommendationEngine(IFeatureBuilder featureBuilder, IEnumerable<IRelatedItemsProvider> relatedItemsReaders)
        {
            _featureBuilder = featureBuilder;
            _relatedItemsReaders = relatedItemsReaders.ToList();
        }

        public IEnumerable<RecommendedItem> Recommend(string userId, int topN)
        {
            var result = new Dictionary<string, RecommendedItem>();
            var features = _featureBuilder.BuildFeatures(userId);
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

                        if (recommendedItem.Reasons.ContainsKey(feature.Id))
                        {
                            recommendedItem.Reasons[feature.Id] += weight;
                        }
                        else
                        {
                            recommendedItem.Reasons.Add(feature.Id, weight);
                        }
                    }
                }
            }

            return result.Values.OrderByDescending(x => x.Weight).Take(topN).ToList();
        }
    }
}