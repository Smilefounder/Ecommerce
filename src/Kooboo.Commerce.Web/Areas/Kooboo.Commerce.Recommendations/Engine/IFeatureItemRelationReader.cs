using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    /// <summary>
    /// 定义通过用户特征获取和该特征相关联的物品的接口方法。
    /// </summary>
    public interface IFeatureItemRelationReader
    {
        /// <summary>
        /// 获取和指定的用户特征相关联的物品及其权重。
        /// </summary>
        IDictionary<string, double> GetRelatedItems(string featureId, int topN);
    }
}