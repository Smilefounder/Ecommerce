using Kooboo.Commerce.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class CommerceInstanceSettingsFile
    {
        private CachedJsonFile<CommerceInstanceSettings> _file;

        public CommerceInstanceSettingsFile(string instanceName)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");
            _file = new CachedJsonFile<CommerceInstanceSettings>(GetPath(instanceName));
        }

        public static string GetPath(string instanceName)
        {
            return Path.Combine(CommerceDataFolder.GetInstancePath(instanceName), "settings.config");
        }

        public CommerceInstanceSettings Read()
        {
            var settings = _file.Read();
            return settings == null ? null : settings.Clone();
        }

        public void Write(CommerceInstanceSettings settings)
        {
            _file.Write(settings);
        }

        public void Delete()
        {
            _file.Delete();
        }
    }
}
