using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class RecommendationOptions
    {
        public ISet<string> ContextItemIds { get; set; }
    }
}