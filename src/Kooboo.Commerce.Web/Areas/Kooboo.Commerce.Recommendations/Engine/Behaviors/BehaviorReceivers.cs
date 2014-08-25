using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public static class BehaviorReceivers
    {
        static readonly Dictionary<string, IBehaviorReceiver> _observersByInstance = new Dictionary<string, IBehaviorReceiver>();

        public static void Set(string instance, IBehaviorReceiver observer)
        {
            if (!_observersByInstance.ContainsKey(instance))
            {
                _observersByInstance.Add(instance, observer);
            }
            else
            {
                _observersByInstance[instance] = observer;
            }
        }

        public static void Remove(string instance)
        {
            _observersByInstance.Remove(instance);
        }

        public static void OnReceive(string instance, IEnumerable<Behavior> behaviors)
        {
            if (_observersByInstance.ContainsKey(instance))
            {
                _observersByInstance[instance].OnReceive(behaviors);
            }
        }
    }
}