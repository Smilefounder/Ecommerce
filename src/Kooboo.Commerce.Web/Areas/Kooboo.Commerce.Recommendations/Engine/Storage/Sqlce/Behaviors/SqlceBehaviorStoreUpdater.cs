using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors
{
    public class SqlceBehaviorStoreUpdater : IBehaviorObserver
    {
        public string Instance { get; private set; }

        public SqlceBehaviorStoreUpdater(string instance)
        {
            Instance =instance;
        }

        public void OnReceive(IEnumerable<Behavior> behaviors)
        {
            foreach (var each in behaviors.GroupBy(it => it.Type))
            {
                OnReceive(each.Key, each.ToList());
            }
        }

        private void OnReceive(string type, IEnumerable<Behavior> behaviors)
        {
            var store = SqlceBehaviorStores.Get(Instance, type);

            // Store behaviors
            store.AddBehaviors(behaviors);

            // Update user/item relation
            foreach (var each in behaviors.GroupBy(it => it.ItemId))
            {
                store.AddItemUsers(each.Key, each.Select(it => it.UserId).Distinct());
            }
            foreach (var each in behaviors.GroupBy(it => it.UserId))
            {
                store.AddUserItems(each.Key, each.Select(it => it.ItemId).Distinct());
            }
        }
    }
}