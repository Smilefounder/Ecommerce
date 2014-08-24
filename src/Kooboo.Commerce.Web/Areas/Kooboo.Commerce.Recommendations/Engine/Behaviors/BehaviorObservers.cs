using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public static class BehaviorObservers
    {
        static readonly Dictionary<string, List<IBehaviorObserver>> _observersByInstance = new Dictionary<string, List<IBehaviorObserver>>();

        public static void Add(string instance, params IBehaviorObserver[] observers)
        {
            if (!_observersByInstance.ContainsKey(instance))
            {
                _observersByInstance.Add(instance, new List<IBehaviorObserver>(observers));
            }
            else
            {
                _observersByInstance[instance].AddRange(observers);
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
                foreach (var observer in _observersByInstance[instance])
                {
                    observer.OnReceive(behaviors);
                }
            }
        }
    }
}