using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Data.Initialization;
using Kooboo.Commerce.Data.Providers;
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
        private List<IInstanceInitializer> _initializers;
        private DataFolderFactory _folderFactory;
        private CommerceInstanceSettingsManager _settingsManager;

        private CommerceDbProviderCollection _dbProviders = CommerceDbProviders.Providers;

        public CommerceDbProviderCollection DbProviders
        {
            get
            {
                return _dbProviders;
            }
            set
            {
                Require.NotNull(value, "value");
                _dbProviders = value;
            }
        }

        public CommerceInstanceManager(DataFolderFactory folderFactory, IEnumerable<IInstanceInitializer> initializers)
        {
            _folderFactory = folderFactory;
            _settingsManager = new CommerceInstanceSettingsManager(_folderFactory);
            _initializers = initializers.ToList();
        }

        public void CreateInstance(CommerceInstanceSettings settings)
        {
            Require.NotNull(settings, "settings");

            var current = _settingsManager.Get(settings.Name);
            if (current != null)
                throw new InvalidOperationException("Commerce instance \"" + settings.Name + "\" already exists.");

            var dbProvider = _dbProviders.Find(settings.DbProviderInvariantName, settings.DbProviderManifestToken);
            var connectionString = dbProvider.GetConnectionString(settings);

            try
            {
                CreatePhysicalDatabaseIfNotExists(connectionString);

                using (var database = new CommerceDatabase(settings))
                {
                    dbProvider.DatabaseOperations.CreateDatabase(database);
                }
            }
            catch (Exception ex)
            {
                throw new CommerceDbException("Commerce instance creation failed: " + ex.Message, ex);
            }

            _settingsManager.Create(settings.Name, settings);

            if (_initializers != null)
            {
                var instance = GetInstance(settings.Name);
                foreach (var initializer in _initializers)
                {
                    initializer.Initialize(instance);
                }
            }

            Event.Raise(new CommerceInstanceCreated(settings));
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
            var settings = _settingsManager.Get(name);
            if (settings == null)
                throw new InvalidOperationException("Cannot find metadata for commerce instance: " + name + ".");

            try
            {
                var dbProvider = _dbProviders.Find(settings.DbProviderInvariantName, settings.DbProviderManifestToken);
                using (var database = new CommerceDatabase(settings))
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

            _settingsManager.Delete(name);

            // Delete instance folder
            var folder = _folderFactory.GetFolder(CommerceDataFolderVirtualPaths.ForInstance(settings.Name), DataFileFormats.Json);
            folder.Delete();

            Event.Raise(new CommerceInstanceDeleted(settings));
        }

        public CommerceInstanceSettings GetInstanceSettings(string instanceName)
        {
            var settingsManager = new CommerceInstanceSettingsManager(_folderFactory);
            return settingsManager.Get(instanceName);
        }

        public IEnumerable<CommerceInstance> GetInstances()
        {
            var instances = new List<CommerceInstance>();
            foreach (var settings in _settingsManager.All())
            {
                instances.Add(new CommerceInstance(settings));
            }
            return instances;
        }

        public CommerceInstance GetInstance(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            var settings = _settingsManager.Get(name);
            if (settings == null)
                throw new InvalidOperationException("Commerce instance \"" + name + "\" not exists.");

            return new CommerceInstance(settings);
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
