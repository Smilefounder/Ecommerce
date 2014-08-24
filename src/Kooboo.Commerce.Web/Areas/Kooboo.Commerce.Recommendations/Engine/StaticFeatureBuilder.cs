using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class StaticFeatureBuilder : IFeatureBuilder
    {
        private List<Feature> _features;

        public StaticFeatureBuilder(IEnumerable<Feature> features)
        {
            _features = features.ToList();
        }

        public IEnumerable<Feature> BuildUserFeatures(string userId)
        {
            return _features;
        }
    }
}