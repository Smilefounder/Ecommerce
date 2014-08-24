using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    /// <summary>
    /// A feature builder that treats user behaved items as the features of the user.
    /// </summary>
    public class BehaviorBasedFeatureBuilder : IFeatureBuilder
    {
        private IUserItemRelationReader _userItemRelationReader;

        public BehaviorBasedFeatureBuilder(IUserItemRelationReader userItemRelationReader)
        {
            _userItemRelationReader = userItemRelationReader;
        }

        public IEnumerable<Feature> BuildUserFeatures(string userId)
        {
            var features = new List<Feature>();
            foreach (var item in _userItemRelationReader.GetItemsBehavedBy(userId))
            {
                features.Add(new Feature
                {
                    Id = item,
                    Weight = 1
                });
            }

            return features;
        }
    }
}