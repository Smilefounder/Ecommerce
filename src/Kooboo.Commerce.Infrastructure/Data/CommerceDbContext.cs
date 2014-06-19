using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Reflection;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.ComponentModel;

namespace Kooboo.Commerce.Data
{
    public class CommerceDbContext : DbContext
    {
        public CommerceInstanceMetadata CommerceInstanceMetadata { get; private set; }

        private CommerceDbContext(CommerceInstanceMetadata commerceInstanceMetadata, string connectionString, DbCompiledModel model)
            : base(connectionString, model)
        {
            CommerceInstanceMetadata = commerceInstanceMetadata;
        }

        public override int SaveChanges()
        {
            var updateObservers = new List<INotifyUpdated>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State.HasFlag(EntityState.Modified))
                {
                    var observer = entry.Entity as INotifyUpdated;
                    if (observer != null)
                    {
                        updateObservers.Add(observer);
                    }
                }
            }

            var result = base.SaveChanges();

            // We must call update notifications after SaveChanges() call, otherwise it's easy to cause infinite loop.
            // Because in current design, each call to Insert, Delete will call SaveChanges(), 
            // and we will always wrap these calls in a tansaction.
            // In this case, SaveChanges will not accept changes, 
            // so if later some other entity are updated, these notifications will still get fired! Causing infinite loop.
            foreach (var observer in updateObservers)
            {
                observer.NotifyUpdated();
            }

            return result;
        }

        static CommerceDbContext()
        {
            Database.SetInitializer<CommerceDbContext>(null);
        }

        static readonly ConcurrentDictionary<ModelCacheKey, DbCompiledModel> _modelCache = new ConcurrentDictionary<ModelCacheKey, DbCompiledModel>();

        internal static CommerceDbContext Create(CommerceInstanceMetadata metadata, ICommerceDbProvider dbProvider)
        {
            var dbProviderInfo = new DbProviderInfo(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);

            var model = _modelCache.GetOrAdd(new ModelCacheKey(metadata.DbSchema, dbProviderInfo), key =>
            {
                return CreateModel(key.Schema, key.DbProviderInfo);
            });

            return new CommerceDbContext(metadata, dbProvider.GetConnectionString(metadata), model);
        }

        static DbCompiledModel CreateModel(string schema, DbProviderInfo dbProviderInfo)
        {
            var builder = new DbModelBuilder();

            if (!String.IsNullOrEmpty(schema))
            {
                builder.HasDefaultSchema(schema);
            }

            builder.Conventions.Remove<PluralizingTableNameConvention>();

            builder.Configurations.AddFromAssembly(Assembly.Load("Kooboo.Commerce.Infrastructure"));
            builder.Configurations.AddFromAssembly(Assembly.Load("Kooboo.Commerce.Data.Mapping"));

            return builder.Build(dbProviderInfo).Compile();
        }

        class ModelCacheKey
        {
            public string Schema { get; private set; }

            public DbProviderInfo DbProviderInfo { get; private set; }

            public ModelCacheKey(string schema, DbProviderInfo dbProviderInfo)
            {
                Require.NotNull(schema, "schema");
                Require.NotNull(dbProviderInfo, "dbProviderInfo");

                DbProviderInfo = dbProviderInfo;
                Schema = schema;
            }

            public override bool Equals(object obj)
            {
                var other = obj as ModelCacheKey;
                return other != null
                    && other.DbProviderInfo.Equals(DbProviderInfo)
                    && other.Schema.Equals(Schema, StringComparison.OrdinalIgnoreCase);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (DbProviderInfo.GetHashCode() * 397) ^ Schema.GetHashCode();
                }
            }
        }
    }
}
