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
        readonly Lazy<Dictionary<string, DataFile>> _slots;

        public string InstanceName { get; private set; }

        public RuleManager(string instanceName)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");

            InstanceName = instanceName;
            _slots = new Lazy<Dictionary<string, DataFile>>(Reload, true);
        }

        private Dictionary<string, DataFile> Reload()
        {
            var dictionary = new Dictionary<string, DataFile>();
            var folder = DataFolders.Instances.GetFolder(InstanceName).GetFolder("Rules", RulesFileFormat.Instance);
            if (folder.Exists)
            {
                foreach (var file in folder.GetFiles("*.config"))
                {
                    var eventName = Path.GetFileNameWithoutExtension(file.Name);
                    // Cache the file content
                    dictionary.Add(eventName, file.Cached());
                }
            }

            return dictionary;
        }

        public IEnumerable<EventSlot> GetSlots()
        {
            return _slots.Value.Values.Select(f => f.Read<EventSlot>()).ToList();
        }

        public IEnumerable<Rule> GetRules(string eventName)
        {
            Require.NotNullOrEmpty(eventName, "eventName");

            DataFile file;

            if (_slots.Value.TryGetValue(eventName, out file))
            {
                var slot = file.Read<EventSlot>();
                if (slot == null)
                {
                    return Enumerable.Empty<Rule>();
                }

                // Here we directly return the result instead of returning a copy
                // is because this method might be used very frequently.
                // So ensure that calling code don't change the result.
                return slot.Rules;
            }

            return Enumerable.Empty<Rule>();
        }

        public void SaveRules(string eventName, IEnumerable<Rule> rules)
        {
            Require.NotNullOrEmpty(eventName, "eventName");

            DataFile file = null;

            if (!_slots.Value.ContainsKey(eventName))
            {
                file = DataFolders.Instances
                                  .GetFolder(InstanceName)
                                  .GetFolder("Rules")
                                  .GetFile(eventName + ".config", RulesFileFormat.Instance)
                                  .Cached(); // Cache file content
                _slots.Value.Add(eventName, file);
            }
            else
            {
                file = _slots.Value[eventName];
            }

            file.Write(new EventSlot(eventName, rules));
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

        class RulesFileFormat : IDataFileFormat
        {
            public static readonly RulesFileFormat Instance = new RulesFileFormat();

            public string Serialize(object content)
            {
                if (content == null)
                {
                    return null;
                }

                return new RuleSerializer().SerializeSlot(content as EventSlot).ToString();
            }

            public object Deserialize(string content, Type type)
            {
                if (String.IsNullOrEmpty(content))
                {
                    return null;
                }

                return new RuleSerializer().DeserializeSlot(content);
            }
        }

        #endregion
    }
}
