using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public interface IFeatureBuilder
    {
        /// <summary>
        /// 构建指定用户的特征并返回。
        /// </summary>
        IEnumerable<Feature> BuildFeatures(string userId);
    }
}