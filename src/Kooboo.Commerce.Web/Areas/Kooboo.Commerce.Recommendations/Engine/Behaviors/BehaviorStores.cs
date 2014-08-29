using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public static class BehaviorStores
    {
        // Key: Instance name, Value Key: Behavior type
        static readonly Dictionary<string, Dictionary<string, IBehaviorStore>> _storesByInstances = new Dictionary<string, Dictionary<string, IBehaviorStore>>();

        public static IBehaviorStore Get(string instance, string behaviorType)
        {
            return _storesByInstances[instance][behaviorType];
        }

        public static void Register(string instance, string behaviorType, IBehaviorStore store)
        {
            if (!_storesByInstances.ContainsKey(instance))
            {
                _storesByInstances.Add(instance, new Dictionary<string, IBehaviorStore>());
            }

            var storesByBehavior = _storesByInstances[instance];
            if (storesByBehavior.ContainsKey(behaviorType))
            {
                storesByBehavior[behaviorType] = store;
            }
            else
            {
                storesByBehavior.Add(behaviorType, store);
            }
        }

        public static void Remove(string instance)
        {
            _storesByInstances.Remove(instance);
        }
    }
}