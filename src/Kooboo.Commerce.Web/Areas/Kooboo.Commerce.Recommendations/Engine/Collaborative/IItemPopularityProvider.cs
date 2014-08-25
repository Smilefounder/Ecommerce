using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    public interface IItemPopularityProvider
    {
        bool IsPopularItem(string itemId);
    }
}