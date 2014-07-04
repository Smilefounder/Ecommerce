using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Rules.Activities
{
    public class ActivityEventManager
    {
        public static ActivityEventManager Instance = new ActivityEventManager();

        private readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private readonly Dictionary<string, EventEntry> _entriesByName = new Dictionary<string, EventEntry>();
        private readonly Dictionary<string, List<EventEntry>> _entriesByCategory = new Dictionary<string, List<EventEntry>>();

        public IEnumerable<string> Categories
        {
            get
            {
                _readWriteLock.EnterReadLock();

                try
                {
                    return _entriesByCategory.Keys;
                }
                finally
                {
                    _readWriteLock.ExitReadLock();
                }
            }
        }

        public bool IsEventRegistered(Type eventType)
        {
            return IsEventRegistered(eventType.Name);
        }

        public bool IsEventRegistered(string eventName)
        {
            _readWriteLock.EnterReadLock();

            try
            {
                return _entriesByName.ContainsKey(eventName);
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public EventEntry FindEvent(Type eventType)
        {
            return FindEvent(eventType.Name);
        }

        public EventEntry FindEvent(string eventName)
        {
            _readWriteLock.EnterReadLock();

            try
            {
                EventEntry entry;

                if (_entriesByName.TryGetValue(eventName, out entry))
                {
                    return entry;
                }

                return null;
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public IEnumerable<EventEntry> FindEvents(string category)
        {
            _readWriteLock.EnterReadLock();

            try
            {
                List<EventEntry> events;

                if (_entriesByCategory.TryGetValue(category, out events))
                {
                    return events.ToList();
                }

                return Enumerable.Empty<EventEntry>();
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public bool RegisterEvent(string category, string displayName, string shortName, Type eventType)
        {
            _readWriteLock.EnterUpgradeableReadLock();

            try
            {
                if (_entriesByName.ContainsKey(eventType.Name))
                {
                    return false;
                }

                _readWriteLock.EnterWriteLock();

                try
                {
                    if (!_entriesByCategory.ContainsKey(category))
                    {
                        _entriesByCategory.Add(category, new List<EventEntry>());
                    }

                    var entry = new EventEntry(category, displayName, shortName, eventType);
                    _entriesByCategory[category].Add(entry);
                    _entriesByName.Add(eventType.Name, entry);

                    return true;
                }
                finally
                {
                    _readWriteLock.ExitWriteLock();
                }
            }
            finally
            {
                _readWriteLock.ExitUpgradeableReadLock();
            }
        }
    }
}
