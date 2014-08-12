using Kooboo.Commerce.Events;
using Kooboo.Commerce.Text;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Rules.Activities
{
    public class ActivityEventManager
    {
        public static ActivityEventManager Instance = new ActivityEventManager();

        private readonly Dictionary<string, EventEntry> _entriesByName = new Dictionary<string, EventEntry>();
        private readonly SortedDictionary<string, List<EventEntry>> _entriesByCategory = new SortedDictionary<string, List<EventEntry>>();

        public IEnumerable<string> Categories
        {
            get
            {
                return _entriesByCategory.Keys;
            }
        }

        public bool IsEventRegistered(Type eventType)
        {
            return IsEventRegistered(eventType.Name);
        }

        public bool IsEventRegistered(string eventName)
        {
            return _entriesByName.ContainsKey(eventName);
        }

        public EventEntry FindEvent(Type eventType)
        {
            return FindEvent(eventType.Name);
        }

        public EventEntry FindEvent(string eventName)
        {
            EventEntry entry;

            if (_entriesByName.TryGetValue(eventName, out entry))
            {
                return entry;
            }

            return null;
        }

        public IEnumerable<EventEntry> FindEvents(string category)
        {
            List<EventEntry> events;

            if (_entriesByCategory.TryGetValue(category, out events))
            {
                return events.OrderBy(e => e.Order).ThenBy(e => e.DisplayName).ToList();
            }

            return Enumerable.Empty<EventEntry>();
        }

        public bool RegisterEvent(Type eventType)
        {
            var attr = eventType.GetCustomAttribute<ActivityEventAttribute>(true);
            if (attr == null)
            {
                return false;
            }

            var category = attr.Category;
            var displayName = attr.DisplayName;
            var shortName = attr.ShortName;

            // Apply conventions
            if (String.IsNullOrEmpty(category))
            {
                var ns = eventType.Namespace;
                if (!String.IsNullOrEmpty(ns))
                {
                    var index = ns.LastIndexOf('.');
                    if (index > 0)
                    {
                        category = ns.Substring(index + 1);
                    }
                }
            }

            if (String.IsNullOrEmpty(displayName))
            {
                displayName = Inflector.Titleize(eventType.Name);
            }

            if (String.IsNullOrEmpty(shortName) && !String.IsNullOrEmpty(category))
            {
                var singular = Inflector.Singularize(category) ?? category;
                if (displayName.StartsWith(singular))
                {
                    shortName = displayName.Substring(singular.Length).Trim();
                }
            }

            if (String.IsNullOrEmpty(shortName))
            {
                shortName = displayName;
            }

            return RegisterEvent(eventType, category, displayName, shortName, attr.Order);
        }

        public bool RegisterEvent(Type eventType, string category, string displayName, string shortName, int order)
        {
            if (_entriesByName.ContainsKey(eventType.Name))
            {
                return false;
            }

            if (!_entriesByCategory.ContainsKey(category))
            {
                _entriesByCategory.Add(category, new List<EventEntry>());
            }

            var entry = new EventEntry(eventType, category, displayName, shortName, order);
            _entriesByCategory[category].Add(entry);
            _entriesByName.Add(eventType.Name, entry);

            return true;
        }

        public void RegisterEvents(IEnumerable<Assembly> assemblies)
        {
            foreach (var asm in assemblies)
            {
                foreach (var type in asm.GetExportedTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(IEvent).IsAssignableFrom(type))
                    {
                        RegisterEvent(type);
                    }
                }
            }
        }
    }
}
