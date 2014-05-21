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
        private const string NullCategory = "<null>";
        private Dictionary<string, HashSet<Type>> _eventsByCategory = new Dictionary<string, HashSet<Type>>();

        public IEnumerable<string> AllCategories()
        {
            return _eventsByCategory.Keys.ToList();
        }

        public IEnumerable<Type> AllEventTypes()
        {
            var types = new HashSet<Type>();
            foreach (var set in _eventsByCategory.Values)
            {
                foreach (var type in set)
                {
                    types.Add(type);
                }
            }

            return types;
        }

        public IEnumerable<Type> FindEventTypesByCategory(string category)
        {
            if (String.IsNullOrEmpty(category))
            {
                category = NullCategory;
            }

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
                        var attr = GetCategoryAttribute(candidate);
                        var category = attr == null ? NullCategory : attr.Name;
                        if (!clone.ContainsKey(category))
                        {
                            clone.Add(category, new HashSet<Type>());
                        }

                        clone[category].Add(candidate);
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

        static CategoryAttribute GetCategoryAttribute(Type eventType)
        {
            var attribute = eventType.GetCustomAttributes(typeof(CategoryAttribute), true)
                                     .OfType<CategoryAttribute>()
                                     .FirstOrDefault();

            if (attribute != null)
            {
                return attribute;
            }

            foreach (var interfaceType in eventType.GetInterfaces())
            {
                attribute = interfaceType.GetCustomAttributes(typeof(CategoryAttribute), true)
                                         .OfType<CategoryAttribute>()
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
