﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Activities
{
    public class EventEntry
    {
        public Type EventType { get; private set; }

        public string Category { get; private set; }

        public string DisplayName { get; private set; }

        public string ShortName { get; private set; }

        public EventEntry(string category, string displayName, string shortName, Type eventType)
        {
            Require.NotNull(eventType, "eventType");

            Category = category;
            DisplayName = displayName;
            ShortName = shortName;
            EventType = eventType;
        }
    }

    public class ActivityEventManager
    {
        public static ActivityEventManager Instance = new ActivityEventManager();

        private readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private readonly Dictionary<Type, EventEntry> _entriesByType = new Dictionary<Type, EventEntry>();
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
            _readWriteLock.EnterReadLock();

            try
            {
                return _entriesByType.ContainsKey(eventType);
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public EventEntry FindEvent(Type eventType)
        {
            _readWriteLock.EnterReadLock();

            try
            {
                EventEntry entry;

                if (_entriesByType.TryGetValue(eventType, out entry))
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
                if (_entriesByType.ContainsKey(eventType))
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
                    _entriesByType.Add(eventType, entry);

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
