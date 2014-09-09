using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    /// <summary>
    /// 基于 Item-to-Item 相似度矩阵实现的特征相关物品读取器，它将用户喜欢的物品作为用户的特征。
    /// </summary>
    public class ItemToItemRelatedItemsProvider : IRelatedItemsProvider
    {
        private ISimilarityMatrix _matrix;

        public ISimilarityMatrix SimilarityMatrix
        {
            get
            {
                return _matrix;
            }
        }

        public ItemToItemRelatedItemsProvider(ISimilarityMatrix matrix)
        {
            _matrix = matrix;
        }

        public IDictionary<string, double> GetRelatedItems(string featureId, int topN, ISet<string> ignoredItems)
        {
            return _matrix.GetMostSimilarItems(featureId, topN, ignoredItems);
        }
    }
}