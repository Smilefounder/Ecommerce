using Kooboo.Commerce.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class CommerceInstanceMetadataFile
    {
        private CachedJsonFile<CommerceInstanceMetadata> _file;

        public CommerceInstanceMetadataFile(string instanceName)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Commerce_Data\\Instances\\" + instanceName + "\\metadata.config");
            _file = new CachedJsonFile<CommerceInstanceMetadata>(path);
        }

        public CommerceInstanceMetadata Read()
        {
            var metadata = _file.Read();
            return metadata == null ? null : metadata.Clone();
        }

        public void Write(CommerceInstanceMetadata metadata)
        {
            _file.Write(metadata);
        }

        public void Delete()
        {
            _file.Delete();
        }
    }
}
