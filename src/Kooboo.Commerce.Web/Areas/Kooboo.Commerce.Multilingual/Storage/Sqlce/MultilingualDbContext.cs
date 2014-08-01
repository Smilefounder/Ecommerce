using Kooboo.Commerce.Data.Folders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServerCompact;
using System.IO;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage.Sqlce
{
    public class MultilingualDbContext : DbContext
    {
        public DbSet<Language> Languages { get; set; }

        public DbSet<EntityTranslationDbEntry> Translations { get; set; }

        public MultilingualDbContext(string instance)
            : base(CreateConnection(instance), Model.Value, true)
        {
        }

        static readonly Lazy<DbCompiledModel> Model = new Lazy<DbCompiledModel>(CreateModel, true);

        static DbCompiledModel CreateModel()
        {
            var builder = new DbModelBuilder();
            builder.Conventions.Remove<PluralizingTableNameConvention>();

            builder.Entity<Language>().HasKey(m => m.Name);
            builder.Entity<EntityTranslationDbEntry>();

            var providerInfo = new DbProviderInfo(SqlCeProviderServices.ProviderInvariantName, "4.0");
            return builder.Build(providerInfo).Compile();
        }

        static DbConnection CreateConnection(string instance)
        {
            var dbProviderInfo = DbConfiguration.DependencyResolver.GetService(typeof(DbProviderFactory), SqlCeProviderServices.ProviderInvariantName) as DbProviderFactory;
            var conn = dbProviderInfo.CreateConnection();
            conn.ConnectionString = GetConnectionString(instance);
            return conn;
        }

        static string GetConnectionString(string instance)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Commerce_Data\\Instances\\" + instance + "\\Multilingual\\Database.sdf");
            return "Data Source=" + path;
        }
    }
}