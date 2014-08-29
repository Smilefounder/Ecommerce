using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    /// <summary>
    /// 定义通过用户特征获取和该特征相关联的物品的接口方法。
    /// </summary>
    public interface IRelatedItemsProvider
    {
        /// <summary>
        /// 获取和指定的特征相关联的物品及其权重。
        /// </summary>
        IDictionary<string, double> GetRelatedItems(string featureId, int topN);
    }

    public static class RelatedItemsProviderExtensions
    {
        public static WeightedRelatedItemsProvider Weighted(this IRelatedItemsProvider provider, float weight)
        {
            return new WeightedRelatedItemsProvider(provider, weight);
        }
    }
}