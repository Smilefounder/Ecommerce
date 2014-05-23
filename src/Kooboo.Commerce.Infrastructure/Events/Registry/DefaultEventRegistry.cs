using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Events.Registry
{
    public class DefaultEventRegistry : IEventRegistry
    {
        private readonly object _registerLock = new object();
        private const string Uncategorized = "Uncategorized";
        private Dictionary<string, List<EventRegistrationEntry>> _eventsByCategory = new Dictionary<string, List<EventRegistrationEntry>>();

        public IEnumerable<string> AllCategories()
        {
            return _eventsByCategory.Keys.ToList();
        }

        public IEnumerable<Type> AllEventTypes()
        {
            var types = new HashSet<Type>();
            foreach (var set in _eventsByCategory.Values)
            {
                foreach (var entry in set)
                {
                    types.Add(entry.EventType);
                }
            }

            return types;
        }

        public IEnumerable<Type> FindEventTypesByCategory(string category)
        {
            if (String.IsNullOrEmpty(category))
            {
                category = Uncategorized;
            }

            // Writes are done with Copy-on-Write, so no need to add read lock here
            List<EventRegistrationEntry> types = null;

            if (_eventsByCategory.TryGetValue(category, out types))
            {
                return types.Select(t => t.EventType).ToList();
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
                        var order = 0;
                        string category = null;
                        var attr = candidate.GetCustomAttribute<EventAttribute>(true);
                        if (attr != null)
                        {
                            order = attr.Order;
                            category = attr.Category;
                        }

                        if (attr == null || String.IsNullOrEmpty(attr.Category))
                        {
                            EventAttribute baseAttr = null;

                            foreach (var @interface in candidate.GetInterfaces())
                            {
                                baseAttr = @interface.GetCustomAttribute<EventAttribute>(true);
                                if (baseAttr != null)
                                {
                                    break;
                                }
                            }

                            if (baseAttr != null)
                            {
                                if (String.IsNullOrEmpty(category))
                                {
                                    category = baseAttr.Category;
                                }
                                if (attr == null)
                                {
                                    order = baseAttr.Order;
                                }
                            }
                        }

                        if (String.IsNullOrEmpty(category))
                        {
                            category = Uncategorized;
                        }

                        if (!clone.ContainsKey(category))
                        {
                            clone.Add(category, new List<EventRegistrationEntry>());
                        }

                        clone[category].Add(new EventRegistrationEntry(candidate, order));
                    }
                }

                foreach (var entries in clone.Values)
                {
                    entries.Sort();
                }

                _eventsByCategory = clone;
            }
        }

        private Dictionary<string, List<EventRegistrationEntry>> CloneInnerDictionary()
        {
            var clone = new Dictionary<string, List<EventRegistrationEntry>>();
            foreach (var kv in _eventsByCategory)
            {
                clone.Add(kv.Key, new List<EventRegistrationEntry>(kv.Value));
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
    }
}
