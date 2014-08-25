using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    public class NullItemPopularityProvider : IItemPopularityProvider
    {
        public static readonly NullItemPopularityProvider Instance = new NullItemPopularityProvider();

        public bool IsPopularItem(string itemId)
        {
            return false;
        }
    }
}