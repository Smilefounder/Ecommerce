using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class CommerceInstanceMetadataManager
    {
        readonly ConcurrentDictionary<string, CommerceInstanceMetadataFile> _metaFilesByInstance = new ConcurrentDictionary<string, CommerceInstanceMetadataFile>();

        public CommerceInstanceMetadata Get(string instanceName)
        {
            CommerceInstanceMetadataFile file;

            if (_metaFilesByInstance.TryGetValue(instanceName, out file))
            {
                return file.Read();
            }

            return null;
        }

        public bool Delete(string instanceName)
        {
            CommerceInstanceMetadataFile file;

            if (_metaFilesByInstance.TryRemove(instanceName, out file))
            {
                file.Delete();
                return true;
            }

            return false;
        }

        public void CreateOrUpdate(string instanceName, CommerceInstanceMetadata metadata)
        {
            var file = _metaFilesByInstance.GetOrAdd(instanceName, name => new CommerceInstanceMetadataFile(name));
            file.Write(metadata);
        }

        public static CommerceInstanceMetadataManager Instance = new CommerceInstanceMetadataManager();
    }
}
