using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.IO;
using Kooboo.Commerce.Rules.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class RuleManager
    {
        readonly string _folderPath;
        readonly Lazy<ConcurrentDictionary<string, CachedFile<EventSlot>>> _slots;

        public string InstanceName { get; private set; }

        public RuleManager(string instanceName)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");

            InstanceName = instanceName;
            _folderPath = CommerceDataFolder.GetInstanceFolderPath(instanceName, "Rules");
            _slots = new Lazy<ConcurrentDictionary<string, CachedFile<EventSlot>>>(Reload, true);
        }

        private ConcurrentDictionary<string, CachedFile<EventSlot>> Reload()
        {
            var dictionary = new ConcurrentDictionary<string, CachedFile<EventSlot>>();
            if (Directory.Exists(_folderPath))
            {
                foreach (var file in Directory.GetFiles(_folderPath, "*.config"))
                {
                    var eventName = Path.GetFileNameWithoutExtension(file);
                    dictionary.TryAdd(eventName, new CachedFile<EventSlot>(file, SerializeSlot, DeserializeSlot));
                }
            }

            return dictionary;
        }

        private string SerializeSlot(EventSlot slot)
        {
            return new RuleSerializer().SerializeSlot(slot).ToString();
        }

        private EventSlot DeserializeSlot(string text)
        {
            return new RuleSerializer().DeserializeSlot(text);
        }

        public IEnumerable<EventSlot> GetSlots()
        {
            return _slots.Value.Values.Select(f => f.Read()).ToList();
        }

        public IEnumerable<RuleBase> GetRules(string eventName)
        {
            Require.NotNullOrEmpty(eventName, "eventName");

            CachedFile<EventSlot> file;

            if (_slots.Value.TryGetValue(eventName, out file))
            {
                var slot = file.Read();
                return slot.Rules;
            }

            return Enumerable.Empty<RuleBase>();
        }

        public void SaveRules(string eventName, IEnumerable<RuleBase> rules)
        {
            Require.NotNullOrEmpty(eventName, "eventName");

            var slot = _slots.Value.GetOrAdd(eventName, name => new CachedFile<EventSlot>(GetEventSlotFilePath(name), SerializeSlot, DeserializeSlot));
            slot.Write(new EventSlot(eventName, rules ?? Enumerable.Empty<RuleBase>()));
        }

        private string GetEventSlotFilePath(string eventName)
        {
            return Path.Combine(_folderPath, eventName + ".config");
        }

        #region Factory

        static readonly ConcurrentDictionary<string, RuleManager> _managers = new ConcurrentDictionary<string, RuleManager>(StringComparer.OrdinalIgnoreCase);

        public static RuleManager GetManager(string instanceName)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");

            return _managers.GetOrAdd(instanceName, name => new RuleManager(name));
        }

        // Remove cache when the instance is deleted
        class RuleManagerCacheUpdateHandler : IHandle<CommerceInstanceDeleted>
        {
            public void Handle(CommerceInstanceDeleted @event)
            {
                RuleManager manager;
                _managers.TryRemove(@event.InstanceSettings.Name, out manager);
            }
        }

        #endregion
    }
}
