using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    /// <summary>
    /// 基于用户行为的特征生成器，它将用户产生了行为的物品当作用户的特征，例如将用户购买的商品当作用户特征。
    /// 在生成特征时，也会加进时间衰减因子，让用户近期产生行为的物品有更高权重。
    /// </summary>
    public class BehaviorBasedFeatureBuilder : IFeatureBuilder
    {
        private Func<IEnumerable<Behavior>> _behaviorsAccessor;

        public Func<DateTime> UtcNow = () => DateTime.UtcNow;

        public float TimeAttenuationAlpha { get; set; }

        public BehaviorBasedFeatureBuilder(Func<IEnumerable<Behavior>> behaviorsAccessor)
        {
            _behaviorsAccessor = behaviorsAccessor;
            TimeAttenuationAlpha = .0002f;
        }

        public IEnumerable<Feature> BuildFeatures(string userId)
        {
            var now = UtcNow();
            var features = new List<Feature>();

            foreach (var behavior in _behaviorsAccessor())
            {
                var weight = behavior.Weight * Formulas.TimeAttenuationFactor(behavior.UtcTimestamp, now, TimeAttenuationAlpha);

                features.Add(new Feature(behavior.ItemId, weight));
            }

            return features;
        }
    }
}