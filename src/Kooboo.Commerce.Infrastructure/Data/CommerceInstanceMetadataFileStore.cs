using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Data
{
    [Dependency(typeof(ICommerceInstanceMetadataStore), ComponentLifeStyle.Singleton)]
    public class CommerceInstanceMetadataFileStore : ICommerceInstanceMetadataStore
    {
        private readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private readonly object _cacheLock = new object();
        private Dictionary<string, CommerceInstanceMetadata> _cache;

        public string FilePath { get; private set; }

        public CommerceInstanceMetadataFileStore()
            : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\Commerce\\Instances.xml"))
        {
        }

        public CommerceInstanceMetadataFileStore(string filePath)
        {
            Require.NotNullOrEmpty(filePath, "filePath");
            FilePath = filePath;
        }

        public IEnumerable<CommerceInstanceMetadata> All()
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

        public CommerceInstanceMetadata GetByName(string name)
        {
            _readWriteLock.EnterReadLock();

            try
            {
                EnsureCacheLoaded();

                CommerceInstanceMetadata metadata;

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

        public void Create(CommerceInstanceMetadata metadata)
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

        public void Update(string name, CommerceInstanceMetadata newMetadata)
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

        private Dictionary<string, CommerceInstanceMetadata> Reload()
        {
            var metadatas = new Dictionary<string, CommerceInstanceMetadata>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(FilePath))
            {
                return metadatas;
            }

            var list = Kooboo.Runtime.Serialization.DataContractSerializationHelper.Deserialize<List<CommerceInstanceMetadata>>(FilePath);

            foreach (var metadata in list)
            {
                metadatas.Add(metadata.Name, metadata);
            }

            return metadatas;
        }

        private void WriteToFile(Dictionary<string, CommerceInstanceMetadata> data)
        {
            var directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Kooboo.Runtime.Serialization.DataContractSerializationHelper.Serialize(data.Values.ToList(), FilePath);
        }

        private Dictionary<string, CommerceInstanceMetadata> GetCacheCopy()
        {
            EnsureCacheLoaded();

            var copy = new Dictionary<string, CommerceInstanceMetadata>(_cache.Comparer);

            foreach (var kv in _cache)
            {
                copy.Add(kv.Key, kv.Value.Clone());
            }

            return copy;
        }
    }
}
