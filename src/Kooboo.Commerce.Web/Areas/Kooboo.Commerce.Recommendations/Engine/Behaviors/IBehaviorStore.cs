using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public interface IBehaviorStore
    {
        IEnumerable<string> GetAllItems();

        DateTime GetBehaviorTimestamp(string userId, string itemId);

        int GetTotalUsersHaveBehaviorsOn(string itemId);

        IEnumerable<string> GetUsersHaveBehaviorsOnBoth(string item1, string item2);

        IEnumerable<Behavior> GetRecentBehaviors(int count);

        int GetUserActiveRate(string userId);

        void SaveBehaviors(IEnumerable<Behavior> behaviors);
    }
}