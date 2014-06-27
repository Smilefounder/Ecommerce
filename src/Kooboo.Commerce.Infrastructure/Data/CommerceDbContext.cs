﻿using System;
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

namespace Kooboo.Commerce.Data
{
    public class CommerceDbContext : DbContext
    {
        public InstanceMetadata CommerceInstanceMetadata { get; private set; }

        private CommerceDbContext(InstanceMetadata commerceInstanceMetadata, string connectionString, DbCompiledModel model)
            : base(connectionString, model)
        {
            CommerceInstanceMetadata = commerceInstanceMetadata;
        }

        public override int SaveChanges()
        {
            Event.Raise(new SavingDbChanges(this));

            return base.SaveChanges();
        }

        static CommerceDbContext()
        {
            Database.SetInitializer<CommerceDbContext>(null);
        }

        static readonly ConcurrentDictionary<ModelCacheKey, DbCompiledModel> _modelCache = new ConcurrentDictionary<ModelCacheKey, DbCompiledModel>();

        internal static CommerceDbContext Create(InstanceMetadata metadata, ICommerceDbProvider dbProvider)
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
