using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    /// <summary>
    /// 基于 Item-to-Item 相似度矩阵实现的特征相关物品读取器，它将用户喜欢的物品作为用户的特征。
    /// </summary>
    public class ItemToItemRelationReader : IFeatureItemRelationReader
    {
        private ISimilarityMatrix _matrix;

        public ItemToItemRelationReader(ISimilarityMatrix matrix)
        {
            _matrix = matrix;
        }

        public IDictionary<string, double> GetRelatedItems(string featureId, int topN)
        {
            return _matrix.GetMostSimilarItems(featureId, topN);
        }
    }
}