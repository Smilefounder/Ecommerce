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

        int GetTotalUsersHadBehaviorsOn(string itemId);

        IEnumerable<string> GetUsersHadBehaviorsOnBoth(string item1, string item2);

        IEnumerable<Behavior> GetRecentBehaviors(int take);

        IEnumerable<string> GetItemsUserHadBehaviorsOn(string userId, int take);

        int GetUserActiveRate(string userId);

        void SaveBehaviors(IEnumerable<Behavior> behaviors);
    }
}