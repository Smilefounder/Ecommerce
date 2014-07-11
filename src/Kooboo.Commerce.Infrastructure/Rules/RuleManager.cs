using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Events;
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
        readonly Lazy<ConcurrentDictionary<string, EventSlot>> _slots;

        public string InstanceName { get; private set; }

        public RuleManager(string instanceName)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");

            InstanceName = instanceName;
            _slots = new Lazy<ConcurrentDictionary<string, EventSlot>>(Reload, true);
        }

        private ConcurrentDictionary<string, EventSlot> Reload()
        {
            var dictionary = new ConcurrentDictionary<string, EventSlot>();
            var folder = new DataFolder(CommerceDataFolderVirtualPaths.ForInstanceFolder(InstanceName, "Rules"), new EventSlotFileFormat());
            if (folder.Exists)
            {
                foreach (var file in folder.GetFiles("*.config"))
                {
                    var eventName = Path.GetFileNameWithoutExtension(file.Name);
                    var slot = file.Read<EventSlot>();
                    dictionary.TryAdd(eventName, slot);
                }
            }

            return dictionary;
        }

        public IEnumerable<EventSlot> GetSlots()
        {
            return _slots.Value.Values.ToList();
        }

        public IEnumerable<RuleBase> GetRules(string eventName)
        {
            Require.NotNullOrEmpty(eventName, "eventName");

            EventSlot slot;

            if (_slots.Value.TryGetValue(eventName, out slot))
            {
                return slot.Rules;
            }

            return Enumerable.Empty<RuleBase>();
        }

        public void SaveRules(string eventName, IEnumerable<RuleBase> rules)
        {
            Require.NotNullOrEmpty(eventName, "eventName");

            var slot = new EventSlot(eventName, rules);
            var file = new DataFile(CommerceDataFolderVirtualPaths.ForInstanceFolder(InstanceName, "Rules/" + eventName + ".config"), new EventSlotFileFormat());
            file.Write(slot);
            _slots.Value.AddOrUpdate(eventName, slot, (_, __) => slot);
        }

        #region Factory

        static readonly ConcurrentDictionary<string, RuleManager> _managers = new ConcurrentDictionary<string, RuleManager>(StringComparer.OrdinalIgnoreCase);

        public static RuleManager GetManager(string instanceName)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");

            return _managers.GetOrAdd(instanceName, name => new RuleManager(name));
        }

        // Remove cache when the instance is deleted
        class CacheUpdateHandler : IHandle<CommerceInstanceDeleted>
        {
            public void Handle(CommerceInstanceDeleted @event)
            {
                RuleManager manager;
                _managers.TryRemove(@event.InstanceSettings.Name, out manager);
            }
        }

        #endregion

        #region Event slot file format

        class EventSlotFileFormat : IDataFileFormat
        {
            public string Serialize(object content)
            {
                if (content == null)
                {
                    return null;
                }

                return new RuleSerializer().SerializeSlot(content as EventSlot).ToString();
            }

            public T Deserialize<T>(string content)
            {
                if (String.IsNullOrEmpty(content))
                {
                    return default(T);
                }

                return (T)(object)new RuleSerializer().DeserializeSlot(content);
            }
        }

        #endregion
    }
}
