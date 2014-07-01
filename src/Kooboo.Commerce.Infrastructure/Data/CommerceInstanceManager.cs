using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Data.Initialization;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    [Dependency(typeof(ICommerceInstanceManager), ComponentLifeStyle.InRequestScope)]
    public class CommerceInstanceManager : ICommerceInstanceManager
    {
        private CommerceDbProviderCollection _dbProviders = CommerceDbProviders.Providers;
        private CommerceInstanceMetadataManager _metadataManager = CommerceInstanceMetadataManager.Instance;

        public CommerceInstanceMetadataManager MetadataManager
        {
            get
            {
                return _metadataManager;
            }
            set
            {
                Require.NotNull(value, "value");
                _metadataManager = value;
            }
        }

        [Inject]
        public IEnumerable<IInstanceInitializer> InstanceInitializers { get; set; }

        public void CreateInstance(CommerceInstanceMetadata metadata)
        {
            Require.NotNull(metadata, "metadata");

            var current = _metadataManager.Get(metadata.Name);
            if (current != null)
                throw new InvalidOperationException("Commerce instance \"" + metadata.Name + "\" already exists.");

            var dbProvider = _dbProviders.Find(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);
            var connectionString = dbProvider.GetConnectionString(metadata);

            try
            {
                CreatePhysicalDatabaseIfNotExists(connectionString);

                using (var database = new CommerceDatabase(metadata))
                {
                    dbProvider.DatabaseOperations.CreateDatabase(database);
                }
            }
            catch(Exception ex)
            {
                throw new CommerceDbException("Commerce instance creation failed: " + ex.Message, ex);
            }

            _metadataManager.CreateOrUpdate(metadata.Name, metadata);

            if (InstanceInitializers != null)
            {
                var instance = GetInstance(metadata.Name);
                foreach (var initializer in InstanceInitializers)
                {
                    initializer.Initialize(instance);
                }
            }
        }

        static void CreatePhysicalDatabaseIfNotExists(string connectionString)
        {
            // TODO: Do not generate __MigrateHistory table or ...?
            using (var dbContext = new EmptyDbContext(connectionString))
            {
                dbContext.Database.CreateIfNotExists();
            }
        }

        public void DeleteInstance(string name)
        {
            var metadata = _metadataManager.Get(name);
            if (metadata == null)
                throw new InvalidOperationException("Cannot find metadata for commerce instance: " + name + ".");

            try
            {
                var dbProvider = _dbProviders.Find(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);
                using (var database = new CommerceDatabase(metadata))
                {
                    if (database.DbContext.Database.Exists())
                    {
                        dbProvider.DatabaseOperations.DeleteDatabase(database);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CommerceDbException("Commerce instance deletion failed: " + ex.Message, ex);
            }

            _metadataManager.Delete(name);
        }

        public CommerceInstanceMetadata GetMetadata(string instanceName)
        {
            return _metadataManager.Get(instanceName);
        }

        public IEnumerable<CommerceInstance> GetInstances()
        {
            var root = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Commerce_Data\\Instances"));
            if (!root.Exists)
            {
                return Enumerable.Empty<CommerceInstance>();
            }

            var instances = new List<CommerceInstance>();

            foreach (var directory in root.GetDirectories())
            {
                var instanceName = directory.Name;
                var metadata = _metadataManager.Get(instanceName);
                // If metadata does not exist, then it's not a valid instance folder.
                if (metadata != null)
                {
                    instances.Add(new CommerceInstance(metadata));
                }
            }

            return instances;
        }

        public CommerceInstance GetInstance(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            var metadata = _metadataManager.Get(name);
            if (metadata == null)
                throw new InvalidOperationException("Commerce instance \"" + name + "\" not exists.");

            return new CommerceInstance(metadata);
        }

        /// <summary>
        /// An empty DbContext which can be used to generate an empty database.
        /// </summary>
        class EmptyDbContext : DbContext
        {
            public EmptyDbContext(string connectionString)
                : base(connectionString)
            {
                Database.SetInitializer<EmptyDbContext>(null);
            }
        }
    }
}
