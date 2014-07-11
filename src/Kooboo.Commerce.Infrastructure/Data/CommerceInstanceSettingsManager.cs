using Kooboo.Commerce.Data.Folders;
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
        const string SettingsFileName = "settings.config";

        public IEnumerable<CommerceInstanceSettings> All()
        {
            var container = new DataFolder(CommerceDataFolderVirtualPaths.Instances);
            foreach (var folder in container.GetFolders())
            {
                var file = folder.GetJsonFile(SettingsFileName);
                if (file.Exists)
                {
                    yield return file.Read<CommerceInstanceSettings>();
                }
            }
        }

        public CommerceInstanceSettings Get(string instanceName)
        {
            var folder = new DataFolder(CommerceDataFolderVirtualPaths.ForInstance(instanceName));
            var file = folder.GetJsonFile(SettingsFileName);
            if (file.Exists)
            {
                return file.Read<CommerceInstanceSettings>();
            }            

            return null;
        }

        public void Create(string instanceName, CommerceInstanceSettings settings)
        {
            var folder = new DataFolder(CommerceDataFolderVirtualPaths.ForInstance(instanceName));
            var file = folder.GetJsonFile(SettingsFileName);
            if (file.Exists)
                throw new InvalidOperationException("Instance settings file already exists. Instance name: " + instanceName + ".");

            file.Write(settings);
        }

        public void Update(string instanceName, CommerceInstanceSettings settings)
        {
            var folder = new DataFolder(CommerceDataFolderVirtualPaths.ForInstance(instanceName));
            var file = folder.GetJsonFile(SettingsFileName);
            if (!file.Exists)
                throw new InvalidOperationException("Failed to update instance metadata because instance was not found. Instance name: " + instanceName + ".");

            if (settings.Name != instanceName)
            {
                settings.Name = instanceName;
            }

            file.Write(settings);
        }

        public void Delete(string instanceName)
        {
            var folder = new DataFolder(CommerceDataFolderVirtualPaths.ForInstance(instanceName));
            var file = folder.GetJsonFile(SettingsFileName);
            if (!file.Exists)
                throw new InvalidOperationException("Instance '" + instanceName + "' was not found.");

            file.Delete();
        }
    }
}
