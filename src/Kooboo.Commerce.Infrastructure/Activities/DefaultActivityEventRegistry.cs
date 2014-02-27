using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class DefaultActivityEventRegistry : IActivityEventRegistry
    {
        private readonly object _registerLock = new object();
        private Dictionary<string, HashSet<Type>> _eventsByCategory = new Dictionary<string,HashSet<Type>>();

        public IEnumerable<string> GetCategories()
        {
            return _eventsByCategory.Keys.ToList();
        }

        public IEnumerable<Type> GetEventTypesByCategory(string category)
        {
            // Writes are done with Copy-on-Write, so no need to add read lock here
            HashSet<Type> types = null;

            if (_eventsByCategory.TryGetValue(category, out types))
            {
                return types;
            }

            return Enumerable.Empty<Type>();
        }

        public void RegisterEvents(IEnumerable<Type> eventTypes)
        {
            lock (_registerLock)
            {
                var clone = CloneInnerDictionary();

                foreach (var candidate in eventTypes)
                {
                    if (candidate.IsClass && !candidate.IsAbstract && typeof(IEvent).IsAssignableFrom(candidate))
                    {
                        var attr = FindActivityVisibleAttribute(candidate);
                        if (attr != null)
                        {
                            if (!clone.ContainsKey(attr.EventCategory))
                            {
                                clone.Add(attr.EventCategory, new HashSet<Type>());
                            }

                            clone[attr.EventCategory].Add(candidate);
                        }
                    }
                }

                _eventsByCategory = clone;
            }
        }

        private Dictionary<string, HashSet<Type>> CloneInnerDictionary()
        {
            var clone = new Dictionary<string, HashSet<Type>>();
            foreach (var kv in _eventsByCategory)
            {
                clone.Add(kv.Key, new HashSet<Type>(kv.Value));
            }

            return clone;
        }

        public void RegisterAssemblies(params System.Reflection.Assembly[] assemblies)
        {
            RegisterAssemblies(assemblies as IEnumerable<Assembly>);
        }

        public void RegisterAssemblies(IEnumerable<System.Reflection.Assembly> assemblies)
        {
            lock (_registerLock)
            {
                var types = new List<Type>();

                foreach (var assembly in assemblies)
                {
                    types.AddRange(assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract));
                }

                if (types.Count > 0)
                {
                    RegisterEvents(types);
                }
            }
        }

        static ActivityVisibleAttribute FindActivityVisibleAttribute(Type eventType)
        {
            var attribute = eventType.GetCustomAttributes(typeof(ActivityVisibleAttribute), true)
                                     .OfType<ActivityVisibleAttribute>()
                                     .FirstOrDefault();

            if (attribute != null)
            {
                return attribute;
            }

            foreach (var interfaceType in eventType.GetInterfaces())
            {
                attribute = interfaceType.GetCustomAttributes(typeof(ActivityVisibleAttribute), true)
                                         .OfType<ActivityVisibleAttribute>()
                                         .FirstOrDefault();

                if (attribute != null)
                {
                    return attribute;
                }
            }

            return null;
        }
    }
}
