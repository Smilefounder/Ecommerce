using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public class BehaviorReceiver : IBehaviorReceiver
    {
        public string Instance { get; private set; }

        public BehaviorReceiver(string instance)
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
            var store = BehaviorStores.Get(Instance, type);
            store.SaveBehaviors(behaviors);
        }
    }
}