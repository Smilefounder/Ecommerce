using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    public class NullItemPopularityReader : IItemPopularityReader
    {
        public static readonly NullItemPopularityReader Instance = new NullItemPopularityReader();

        public bool IsPopularItem(string itemId)
        {
            return false;
        }
    }
}