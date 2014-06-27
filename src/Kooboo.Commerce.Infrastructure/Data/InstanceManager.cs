using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Data.Initialization;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    [Dependency(typeof(IInstanceManager), ComponentLifeStyle.InRequestScope)]
    public class InstanceManager : IInstanceManager
    {
        private IInstanceMetadataStore _metadataStore;
        private CommerceDbProviderCollection _dbProviders = CommerceDbProviders.Providers;

        [Inject]
        public IEnumerable<IInstanceInitializer> InstanceInitializers { get; set; }

        public InstanceManager(IInstanceMetadataStore metadataStore)
        {
            Require.NotNull(metadataStore, "metadataStore");
            _metadataStore = metadataStore;
        }

        public void CreateInstance(InstanceMetadata metadata)
        {
            Require.NotNull(metadata, "metadata");

            var current = _metadataStore.GetByName(metadata.Name);
            if (current != null)
                throw new InvalidOperationException("Commerce instance \"" + metadata.Name + "\" already exists.");

            var dbProvider = _dbProviders.Find(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);
            var connectionString = dbProvider.GetConnectionString(metadata);

            try
            {
                CreatePhysicalDatabaseIfNotExists(connectionString);

                using (var database = new CommerceDatabase(metadata, dbProvider))
                {
                    dbProvider.DatabaseOperations.CreateDatabase(database);
                }
            }
            catch(Exception ex)
            {
                throw new CommerceDbException("Commerce instance creation failed: " + ex.Message, ex);
            }

            _metadataStore.Create(metadata);

            if (InstanceInitializers != null)
            {
                var instance = OpenInstance(metadata.Name);
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
            var metadata = _metadataStore.GetByName(name);
            if (metadata == null)
                throw new InvalidOperationException("Cannot find metadata for commerce instance: " + name + ".");

            try
            {
                var dbProvider = _dbProviders.Find(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);
                using (var database = new CommerceDatabase(metadata, dbProvider))
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

            _metadataStore.Delete(name);
        }

        public InstanceMetadata GetInstanceMetadata(string instanceName)
        {
            return _metadataStore.GetByName(instanceName);
        }

        public IEnumerable<InstanceMetadata> GetAllInstanceMetadatas()
        {
            return _metadataStore.All().ToList();
        }

        public CommerceInstance OpenInstance(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            var metadata = _metadataStore.GetByName(name);
            if (metadata == null)
                throw new InvalidOperationException("Commerce instance \"" + name + "\" not exists.");

            var dbProvider = _dbProviders.Find(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);
            var database = new CommerceDatabase(metadata, dbProvider);

            return new CommerceInstance(database);
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
