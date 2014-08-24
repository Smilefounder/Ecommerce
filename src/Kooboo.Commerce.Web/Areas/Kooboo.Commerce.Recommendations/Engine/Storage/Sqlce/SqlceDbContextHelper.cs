using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServerCompact;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce
{
    static class SqlceDbContextHelper
    {
        static readonly ConcurrentDictionary<string, DbCompiledModel> _cache = new ConcurrentDictionary<string, DbCompiledModel>();

        public static DbCompiledModel GetModel(string instance, string dbName, Action<DbModelBuilder> configure)
        {
            return _cache.GetOrAdd(instance + "|" + dbName, _ => CreateModel(configure));
        }

        static DbCompiledModel CreateModel(Action<DbModelBuilder> configure)
        {
            var builder = new DbModelBuilder();
            //builder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            configure(builder);

            var providerInfo = new DbProviderInfo(SqlCeProviderServices.ProviderInvariantName, "4.0");
            return builder.Build(providerInfo).Compile();
        }

        public static DbConnection CreateConnection(string instance, string dbName)
        {
            var dbProviderInfo = DbConfiguration.DependencyResolver.GetService(typeof(DbProviderFactory), SqlCeProviderServices.ProviderInvariantName) as DbProviderFactory;
            var conn = dbProviderInfo.CreateConnection();
            conn.ConnectionString = GetConnectionString(instance, dbName);
            return conn;
        }

        public static string GetConnectionString(string instance, string dbName)
        {
            return "Data Source=" + Paths.Database(instance, dbName);
        }
    }
}