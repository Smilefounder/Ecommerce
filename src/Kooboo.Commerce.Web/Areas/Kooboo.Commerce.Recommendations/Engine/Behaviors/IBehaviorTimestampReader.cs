using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public interface IBehaviorTimestampReader
    {
        DateTime GetBehaviorTimestamp(string userId, string itemId);
    }
}