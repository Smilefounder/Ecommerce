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
using Kooboo.Commerce.Data.Providers;
using Kooboo.Commerce.Data.Mapping;
using System.Data.Common;

namespace Kooboo.Commerce.Data
{
    public class CommerceDbContext : DbContext
    {
        public CommerceInstanceSettings CommerceInstanceMetadata { get; private set; }

        private CommerceDbContext(CommerceInstanceSettings commerceInstanceMetadata, DbConnection connection, DbCompiledModel model)
            : base(connection, model, true)
        {
            CommerceInstanceMetadata = commerceInstanceMetadata;
        }

        public override int SaveChanges()
        {
            // TODO: Pass in commerce instance
            Event.Raise(new SavingDbChanges(this), new EventContext());

            // Delete orphans
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified && e.Entity is IOrphanable);
            foreach (var entry in entries)
            {
                var orphanable = entry.Entity as IOrphanable;
                if (orphanable.IsOrphan())
                {
                    entry.State = EntityState.Deleted;
                }
            }

            return base.SaveChanges();
        }

        static CommerceDbContext()
        {
            Database.SetInitializer<CommerceDbContext>(null);
        }

        static readonly ConcurrentDictionary<ModelCacheKey, DbCompiledModel> _modelCache = new ConcurrentDictionary<ModelCacheKey, DbCompiledModel>();

        internal static CommerceDbContext Create(CommerceInstanceSettings metadata, ICommerceDbProvider dbProvider)
        {
            var dbProviderInfo = new DbProviderInfo(metadata.DbProviderInvariantName, metadata.DbProviderManifestToken);

            var model = _modelCache.GetOrAdd(new ModelCacheKey(metadata.DbSchema, dbProviderInfo), key =>
            {
                return CreateModel(key.Schema, key.DbProviderInfo);
            });

            var dbProviderFactory = DbConfiguration.DependencyResolver.GetService(typeof(DbProviderFactory), dbProvider.InvariantName) as DbProviderFactory;
            var conn = dbProviderFactory.CreateConnection();
            conn.ConnectionString = dbProvider.GetConnectionString(metadata);

            return new CommerceDbContext(metadata, conn, model);
        }

        static DbCompiledModel CreateModel(string schema, DbProviderInfo dbProviderInfo)
        {
            var builder = new DbModelBuilder();

            if (!String.IsNullOrEmpty(schema))
            {
                builder.HasDefaultSchema(schema);
            }

            builder.Conventions.Remove<PluralizingTableNameConvention>();
            builder.Conventions.Add<NonPublicPropertyConvention>();

            builder.Configurations.AddFromAssembly(Assembly.Load("Kooboo.Commerce.Infrastructure"));
            builder.Configurations.AddFromAssembly(Assembly.Load("Kooboo.Commerce"));

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
