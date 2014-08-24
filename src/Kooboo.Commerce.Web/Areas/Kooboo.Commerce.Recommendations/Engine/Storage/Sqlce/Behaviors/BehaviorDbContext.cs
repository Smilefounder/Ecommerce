using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors
{
    public class BehaviorDbContext : DbContext
    {
        public DbSet<BehaviorRecord> Behaviors { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<ItemUsers> ItemUsers { get; set; }

        public DbSet<UserItems> UserItems { get; set; }

        public BehaviorDbContext(string instance, string behaviorType)
            : base(SqlceDbContextHelper.CreateConnection(instance, "Behaviors_" + behaviorType), GetModel(instance, "Behaviors_" + behaviorType), true)
        {
        }

        static DbCompiledModel GetModel(string instance, string dbName)
        {
            return SqlceDbContextHelper.GetModel(instance, dbName, builder =>
            {
                builder.Entity<BehaviorRecord>();
                builder.Entity<Item>();
                builder.Entity<ItemUsers>();
                builder.Entity<UserItems>();
            });
        }
    }
}