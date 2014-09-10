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
        private readonly Lazy<Dictionary<string, DataFile>> _eventFiles;
        private EventSlotManager _slotManager = EventSlotManager.Instance;

        public string InstanceName { get; private set; }

        public RuleManager(string instanceName)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");

            InstanceName = instanceName;
            _eventFiles = new Lazy<Dictionary<string, DataFile>>(Reload, true);
        }

        private Dictionary<string, DataFile> Reload()
        {
            var dictionary = new Dictionary<string, DataFile>();
            var folder = DataFolders.Instances.GetFolder(InstanceName).GetFolder("Rules");
            if (folder.Exists)
            {
                foreach (var file in folder.GetFiles("*.config"))
                {
                    var eventName = Path.GetFileNameWithoutExtension(file.Name);
                    var slot = _slotManager.GetSlot(eventName);
                    file.Format = new RulesFileFormat(slot);
                    dictionary.Add(eventName, file.Cached()); // Cache file content
                }
            }

            return dictionary;
        }

        public IEnumerable<Rule> GetRules(string eventName)
        {
            Require.NotNullOrEmpty(eventName, "eventName");

            DataFile file;

            if (_eventFiles.Value.TryGetValue(eventName, out file))
            {
                return file.Read<IList<Rule>>();
            }

            return Enumerable.Empty<Rule>();
        }

        public void SaveRules(string eventName, IEnumerable<Rule> rules)
        {
            Require.NotNullOrEmpty(eventName, "eventName");

            DataFile file = null;

            if (!_eventFiles.Value.ContainsKey(eventName))
            {
                var slot = _slotManager.GetSlot(eventName);

                file = DataFolders.Instances
                                  .GetFolder(InstanceName)
                                  .GetFolder("Rules")
                                  .GetFile(eventName + ".config", new RulesFileFormat(slot))
                                  .Cached(); // Cache file content
                _eventFiles.Value.Add(eventName, file);
            }
            else
            {
                file = _eventFiles.Value[eventName];
            }

            file.Write(rules);
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
                _managers.TryRemove(@event.Settings.Name, out manager);
            }
        }

        #endregion

        #region Event slot file format

        class RulesFileFormat : IDataFileFormat
        {
            public EventSlot Slot { get; set; }

            public RulesFileFormat(EventSlot slot)
            {
                Slot = slot;
            }

            public string Serialize(object content)
            {
                if (content == null)
                {
                    return null;
                }

                return new RuleSerializer().SerializeRules(content as IEnumerable<Rule>).ToString();
            }

            public object Deserialize(string content, Type type)
            {
                if (String.IsNullOrEmpty(content))
                {
                    return null;
                }

                return new RuleSerializer().DeserializeRules(Slot, content);
            }
        }

        #endregion
    }
}
