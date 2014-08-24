using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public interface IFeatureBuilder
    {
        IEnumerable<Feature> BuildUserFeatures(string userId);
    }
}