using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    public interface IItemPopularityReader
    {
        bool IsPopularItem(string itemId);
    }
}