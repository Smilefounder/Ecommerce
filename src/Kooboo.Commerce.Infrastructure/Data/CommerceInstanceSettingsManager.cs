using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class CommerceInstanceSettingsManager
    {
        readonly Dictionary<string, CommerceInstanceSettingsFile> _settingsFilesByInstance = new Dictionary<string, CommerceInstanceSettingsFile>();

        public IEnumerable<CommerceInstanceSettings> All()
        {
            return _settingsFilesByInstance.Values.Select(f => f.Read()).ToList();
        }

        public CommerceInstanceSettings Get(string instanceName)
        {
            CommerceInstanceSettingsFile file;

            if (_settingsFilesByInstance.TryGetValue(instanceName, out file))
            {
                return file.Read();
            }

            return null;
        }

        public void Create(string instanceName, CommerceInstanceSettings settings)
        {
            if (_settingsFilesByInstance.ContainsKey(instanceName))
                throw new InvalidOperationException("Instance settings file already exists. Instance name: " + instanceName + ".");

            var file = new CommerceInstanceSettingsFile(instanceName);
            _settingsFilesByInstance.Add(instanceName, file);
            file.Write(settings);
        }

        public void Update(string instanceName, CommerceInstanceSettings settings)
        {
            if (!_settingsFilesByInstance.ContainsKey(instanceName))
                throw new InvalidOperationException("Failed to update instance metadata because instance was not found. Instance name: " + instanceName + ".");

            if (settings.Name != instanceName)
                throw new InvalidOperationException("Cannot change instance name.");

            var file = _settingsFilesByInstance[instanceName];
            file.Write(settings);
        }

        public void Delete(string instanceName)
        {
            if (!_settingsFilesByInstance.ContainsKey(instanceName))
                throw new InvalidOperationException("Instance '" + instanceName + "' was not found.");

            var file = _settingsFilesByInstance[instanceName];
            file.Delete();

            _settingsFilesByInstance.Remove(instanceName);
        }

        static readonly Lazy<CommerceInstanceSettingsManager> _instance;

        public static CommerceInstanceSettingsManager Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        static CommerceInstanceSettingsManager()
        {
            _instance = new Lazy<CommerceInstanceSettingsManager>(Reload, true);
        }

        static CommerceInstanceSettingsManager Reload()
        {
            var manager = new CommerceInstanceSettingsManager();
            var root = new DirectoryInfo(CommerceDataFolder.InstancesRootPath);
            if (root.Exists)
            {
                foreach (var directory in root.GetDirectories())
                {
                    var settingsFilePath = CommerceInstanceSettingsFile.GetPath(directory.Name);
                    if (File.Exists(settingsFilePath))
                    {
                        var file = new CommerceInstanceSettingsFile(directory.Name);
                        manager._settingsFilesByInstance.Add(directory.Name, file);
                    }
                }
            }

            return manager;
        }
    }
}
