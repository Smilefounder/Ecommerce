using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    [Dependency(typeof(ICommerceInstanceManager), ComponentLifeStyle.InRequestScope)]
    public class CommerceInstanceManager : ICommerceInstanceManager
    {
        private ICommerceInstanceMetadataStore _metadataStore;
        private ICommerceDbProviderFactory _dbProviderFactory;

        public CommerceInstanceManager(
            ICommerceInstanceMetadataStore metadataStore,
            ICommerceDbProviderFactory dbProviderFactory)
        {
            Require.NotNull(metadataStore, "metadataStore");
            Require.NotNull(dbProviderFactory, "dbProviderFactory");

            _metadataStore = metadataStore;
            _dbProviderFactory = dbProviderFactory;
        }

        public void CreateInstance(CommerceInstanceMetadata metadata)
        {
            Require.NotNull(metadata, "metadata");

            var current = _metadataStore.GetByName(metadata.Name);
            if (current != null)
                throw new InvalidOperationException("Commerce instance \"" + metadata.Name + "\" already exists.");

            var dbProvider = _dbProviderFactory.GetDbProvider(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);
            var connectionString = dbProvider.GetConnectionString(metadata);

            Event.Raise(new CommerceInstanceCreating(metadata));

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

            Event.Raise(new CommerceInstanceCreated(metadata));
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

            Event.Raise(new CommerceInstanceDeleting(metadata));

            try
            {
                var dbProvider = _dbProviderFactory.GetDbProvider(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);
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

            Event.Raise(new CommerceInstanceDeleted(metadata));
        }

        public CommerceInstanceMetadata GetInstanceMetadata(string instanceName)
        {
            return _metadataStore.GetByName(instanceName);
        }

        public IEnumerable<CommerceInstanceMetadata> GetAllInstanceMetadatas()
        {
            return _metadataStore.All().ToList();
        }

        public CommerceInstance OpenInstance(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            var metadata = _metadataStore.GetByName(name);
            if (metadata == null)
                throw new InvalidOperationException("Commerce instance \"" + name + "\" not exists.");

            var dbProvider = _dbProviderFactory.GetDbProvider(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);
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
