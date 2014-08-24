using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors
{
    public static class SqlceBehaviorStores
    {
        // Key: Instance name, Value Key: Behavior type
        static readonly Dictionary<string, Dictionary<string, SqlceBehaviorStore>> _storesByInstances = new Dictionary<string, Dictionary<string, SqlceBehaviorStore>>();

        public static SqlceBehaviorStore Get(string instance, string behaviorType)
        {
            return _storesByInstances[instance][behaviorType];
        }

        public static void Set(string instance, string behaviorType, SqlceBehaviorStore store)
        {
            if (!_storesByInstances.ContainsKey(instance))
            {
                _storesByInstances.Add(instance, new Dictionary<string, SqlceBehaviorStore>());
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