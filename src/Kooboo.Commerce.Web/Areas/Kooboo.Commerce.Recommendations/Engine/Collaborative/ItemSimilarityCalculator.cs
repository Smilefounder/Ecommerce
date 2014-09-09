using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    /// <summary>
    /// 用于计算两物品相似度。
    /// </summary>
    /// <remarks>
    /// 原始公式为: 
    /// similarity(i, j) = N(i) ∩ N(j) / SQRT(N(i) * N(j))
    /// 
    /// 其中 N(i) 为对 i 产生过行为的用户数，N(j) 为对 j 产生过行为的用户数。
    /// 
    /// 改进方案为:
    /// (1) 引入时间衰减因子。原先分子为共同对i, j产生行为的用户数，即对两物品每产生一次共同物品则加固定数值1，将其改为每次加上 1 * 时间衰减因子。
    ///     时间衰减因子的计算方法为: time attenuation factor = 1 / (1 + alpha * Abs(timestamp1 - timestamp2))，
    ///     其中 alpha 由 TimeAttenuationAlpha 指定，值越大衰减越快。
    ///     
    /// (2) 对流行物品进行处罚。原先分母为 SQRT(N(i) * N(j))，即 N(i) ^ 0.5 * N(j) ^ 0.5，若 j 为流行物品，则公式分母改为 N(i) ^ (1 - alpha) * N(j) ^ alpha，
    ///     其中 alpha 由 PopularItemPunishAlpha 指定，值越大处罚的越多，当为 0.5 时和原公式等价。
    ///     
    /// (3) 对活跃用户进行处罚 (ItemCF-IUF)。在计算分子时，改为当用户对两物品有共同行为时，乘上 1 / Log(1 + 用户活跃度)。这条改进和 (1) 相乘共同构成分子的值。
    /// </remarks>
    public class ItemSimilarityCalculator
    {
        private IBehaviorStore _behaviorStore;
        private IItemPopularityProvider _popularityProvider;

        /// <summary>
        /// 时间衰减因子公式中的参数Alpha，值越大，则时间间隔越久产生行为的物品相似度越小。
        /// </summary>
        public float TimeAttenuationAlpha { get; set; }

        /// <summary>
        /// 流行物品处罚时使用的参数Alpha，值越大，热门物品对相似度的贡献下降的越多。
        /// </summary>
        public float PopularItemPunishAlpha { get; set; }

        public ItemSimilarityCalculator(IBehaviorStore behaviorStore, IItemPopularityProvider popularityProvider)
        {
            _behaviorStore = behaviorStore;
            _popularityProvider = popularityProvider;

            TimeAttenuationAlpha = .0002f;
            PopularItemPunishAlpha = .6f;
        }

        public double CalculateSimilarity(string item1, string item2)
        {
            var usersBehavedOnBothItems = _behaviorStore.GetUsersHadBehaviorsOnBoth(item1, item2);
            var numerator = ComputeNumerator(item1, item2, usersBehavedOnBothItems);
            var denorminator = ComputeDenominator(item1, item2);

            if (denorminator == 0)
            {
                return 0;
            }

            return numerator / denorminator;
        }

        private double ComputeNumerator(string item1, string item2, IEnumerable<string> usersBehavedOnBothItems)
        {
            double finalNumerator = 0d;

            foreach (var userId in usersBehavedOnBothItems)
            {
                double numerator = 1d;

                // 处罚活跃用户，越活跃的用户对相似度的贡献应越小。
                // 例如，一个用户对90%的物品产生过行为，那这些行为对推荐的意义便不是太大
                numerator = 1 / Math.Log(1 + _behaviorStore.GetUserActiveRate(userId));

                // 乘上时间衰减因子，同一个用户在越接近的时间里对两个物品产生了行为，此行为对相似度的贡献应越大。
                // 例如，用户5分钟内产生行为的两个物品相似度较大，而相隔了几个月产生共同行为的两个物品相似度应很小。
                var timestamp1 = _behaviorStore.GetBehaviorTimestamp(userId, item1);
                var timestamp2 = _behaviorStore.GetBehaviorTimestamp(userId, item2);
                var attenuationFactor = Formulas.TimeAttenuationFactor(timestamp1, timestamp2, TimeAttenuationAlpha);
                numerator *= attenuationFactor;

                finalNumerator += numerator;
            }

            return finalNumerator;
        }

        private double ComputeDenominator(string item1, string item2)
        {
            var totalBehavingUsersOfItem1 = _behaviorStore.GetTotalUsersHadBehaviorsOn(item1);
            var totalBehavingUsersOfItem2 = _behaviorStore.GetTotalUsersHadBehaviorsOn(item2);

            var power1 = .5f;
            var power2 = .5f;

            var isItem1Popular = _popularityProvider.IsPopularItem(item1);
            var isItem2Popular = _popularityProvider.IsPopularItem(item2);

            // 如果两个物品为冷门物品对热门物品，则处罚热门物品
            if (isItem1Popular != isItem2Popular)
            {
                if (isItem1Popular)
                {
                    power1 = PopularItemPunishAlpha;
                    power2 = 1 - PopularItemPunishAlpha;
                }
                else
                {
                    power1 = 1 - PopularItemPunishAlpha;
                    power2 = PopularItemPunishAlpha;
                }
            }

            return Math.Pow(totalBehavingUsersOfItem1, power1) * Math.Pow(totalBehavingUsersOfItem2, power2);
        }
    }
}