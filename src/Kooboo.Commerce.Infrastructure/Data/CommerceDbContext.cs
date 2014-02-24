using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Reflection;

namespace Kooboo.Commerce.Data
{
    public class CommerceDbContext : DbContext
    {
        public string Schema { get; private set; }

        private CommerceDbContext(string schema, string connectionString, DbCompiledModel model)
            : base(connectionString, model)
        {
            Schema = schema;
        }

        static CommerceDbContext()
        {
            Database.SetInitializer<CommerceDbContext>(null);
        }

        public void InitializeDatabase()
        {
            if (!Database.Exists())
            {
                Database.Create();
            }
            else
            {
                var dbScript = ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
                Database.ExecuteSqlCommand(dbScript);
            }
        }

        static readonly ConcurrentDictionary<ModelCacheKey, DbCompiledModel> _modelCache = new ConcurrentDictionary<ModelCacheKey, DbCompiledModel>();

        public static CommerceDbContext Create(string schema, string connectionString, DbProviderInfo dbProviderInfo)
        {
            schema = schema ?? String.Empty;

            var model = _modelCache.GetOrAdd(new ModelCacheKey(schema, dbProviderInfo), key =>
            {
                return CreateModel(key.Schema, key.DbProviderInfo);
            });

            return new CommerceDbContext(schema, connectionString, model);
        }

        static DbCompiledModel CreateModel(string schema, DbProviderInfo dbProviderInfo)
        {
            var builder = new DbModelBuilder();

            if (!String.IsNullOrEmpty(schema))
            {
                builder.HasDefaultSchema(schema);
            }

            builder.Conventions.Remove<PluralizingTableNameConvention>();
            // TODO: Might need to open mapping assembly loading as an extension point
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
