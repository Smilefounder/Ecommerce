using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    /// <summary>
    /// 被推荐给用户的物品。
    /// </summary>
    public class RecommendedItem
    {
        public string ItemId { get; set; }

        public double Weight { get; set; }

        /// <summary>
        /// 物品被推荐的原因，值为权重，在基于特征的推荐引擎中，键是特征Id。
        /// </summary>
        public IDictionary<string, double> Reasons { get; set; }

        public RecommendedItem()
        {
            Reasons = new Dictionary<string, double>();
        }

        public override string ToString()
        {
            return String.Format("ItemId: {0}, Weight: {1}", ItemId, Weight);
        }
    }
}