using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Data
{
    [Dependency(typeof(IInstanceMetadataStore), ComponentLifeStyle.Singleton)]
    public class InstanceMetadataFileStore : IInstanceMetadataStore
    {
        private readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private readonly object _cacheLock = new object();
        private Dictionary<string, InstanceMetadata> _cache;

        public string FilePath { get; private set; }

        public InstanceMetadataFileStore()
            : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\Commerce\\Instances.xml"))
        {
        }

        public InstanceMetadataFileStore(string filePath)
        {
            Require.NotNullOrEmpty(filePath, "filePath");
            FilePath = filePath;
        }

        public IEnumerable<InstanceMetadata> All()
        {
            _readWriteLock.EnterReadLock();

            try
            {
                EnsureCacheLoaded();
                return _cache.Values.Select(x => x.Clone()).ToList();
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public InstanceMetadata GetByName(string name)
        {
            _readWriteLock.EnterReadLock();

            try
            {
                EnsureCacheLoaded();

                InstanceMetadata metadata;

                if (_cache.TryGetValue(name, out metadata))
                {
                    return metadata.Clone();
                }

                return null;
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public void Create(InstanceMetadata metadata)
        {
            _readWriteLock.EnterWriteLock();

            try
            {
                var copy = GetCacheCopy();
                copy.Add(metadata.Name, metadata);

                WriteToFile(copy);

                _cache = copy;
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public void Update(string name, InstanceMetadata newMetadata)
        {
            _readWriteLock.EnterWriteLock();

            try
            {
                EnsureCacheLoaded();

                if (!_cache.ContainsKey(name))
                    throw new InvalidOperationException("Commerce instance \"" + name + "\" not exists.");

                var copy = GetCacheCopy();
                var oldMetadata = copy[name];

                oldMetadata.DisplayName = newMetadata.DisplayName;
                oldMetadata.ConnectionString = newMetadata.ConnectionString;

                WriteToFile(copy);

                _cache = copy;
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public void Delete(string name)
        {
            _readWriteLock.EnterWriteLock();

            try
            {
                var copy = GetCacheCopy();
                copy.Remove(name);

                WriteToFile(copy);

                _cache = copy;
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        private void EnsureCacheLoaded()
        {
            if (_cache == null)
            {
                lock (_cacheLock)
                {
                    if (_cache == null)
                    {
                        _cache = Reload();
                    }
                }
            }
        }

        private Dictionary<string, InstanceMetadata> Reload()
        {
            var metadatas = new Dictionary<string, InstanceMetadata>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(FilePath))
            {
                return metadatas;
            }

            var list = Kooboo.Runtime.Serialization.DataContractSerializationHelper.Deserialize<List<InstanceMetadata>>(FilePath);

            foreach (var metadata in list)
            {
                metadatas.Add(metadata.Name, metadata);
            }

            return metadatas;
        }

        private void WriteToFile(Dictionary<string, InstanceMetadata> data)
        {
            var directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Kooboo.Runtime.Serialization.DataContractSerializationHelper.Serialize(data.Values.ToList(), FilePath);
        }

        private Dictionary<string, InstanceMetadata> GetCacheCopy()
        {
            EnsureCacheLoaded();

            var copy = new Dictionary<string, InstanceMetadata>(_cache.Comparer);

            foreach (var kv in _cache)
            {
                copy.Add(kv.Key, kv.Value.Clone());
            }

            return copy;
        }
    }
}
