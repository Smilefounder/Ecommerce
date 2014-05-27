using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Events.Registry
{
    public class DefaultEventRegistry : IEventRegistry
    {
        private readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private const string Uncategorized = "Uncategorized";
        private Dictionary<Type, EventRegistrationEntry> _entriesByType = new Dictionary<Type, EventRegistrationEntry>();
        private Dictionary<string, List<EventRegistrationEntry>> _entriesByCategory = new Dictionary<string, List<EventRegistrationEntry>>();

        public IEnumerable<string> AllCategories()
        {
            _readWriteLock.EnterReadLock();

            try
            {
                return _entriesByCategory.Keys.ToList();
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public IEnumerable<EventRegistrationEntry> AllEvents()
        {
            _readWriteLock.EnterReadLock();

            try
            {
                return _entriesByType.Values;
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public EventRegistrationEntry FindByType(Type eventType)
        {
            _readWriteLock.EnterReadLock();

            try
            {
                EventRegistrationEntry entry = null;
                _entriesByType.TryGetValue(eventType, out entry);

                return entry;
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public IEnumerable<EventRegistrationEntry> FindByCategory(string category)
        {
            if (String.IsNullOrEmpty(category))
            {
                category = Uncategorized;
            }

            // Writes are done with Copy-on-Write, so no need to add read lock here
            List<EventRegistrationEntry> types = null;

            _readWriteLock.EnterReadLock();

            try
            {
                if (!_entriesByCategory.TryGetValue(category, out types))
                {
                    types = new List<EventRegistrationEntry>();
                }
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }

            return types;
        }

        public void RegisterEvents(IEnumerable<Type> eventTypes)
        {
            _readWriteLock.EnterWriteLock();

            try
            {
                var entriesByCategory = _entriesByCategory;
                var entriesByType = _entriesByType;

                foreach (var candidate in eventTypes)
                {
                    if (candidate.IsClass && !candidate.IsAbstract && typeof(IEvent).IsAssignableFrom(candidate))
                    {
                        if (entriesByType.ContainsKey(candidate))
                        {
                            continue;
                        }

                        var entry = new EventRegistrationEntry(candidate);
                        entriesByType.Add(candidate, entry);

                        var category = String.IsNullOrEmpty(entry.Category) ? Uncategorized : entry.Category;

                        if (!entriesByCategory.ContainsKey(category))
                        {
                            entriesByCategory.Add(category, new List<EventRegistrationEntry>());
                        }

                        entriesByCategory[category].Add(entry);
                    }
                }

                foreach (var entries in entriesByCategory.Values)
                {
                    entries.Sort();
                }

                _entriesByCategory = entriesByCategory;
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public void RegisterAssemblies(params System.Reflection.Assembly[] assemblies)
        {
            RegisterAssemblies(assemblies as IEnumerable<Assembly>);
        }

        public void RegisterAssemblies(IEnumerable<System.Reflection.Assembly> assemblies)
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
