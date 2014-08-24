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

        public DbSet<ItemUsers> ItemUsers { get; set; }

        public DbSet<UserItems> UserItems { get; set; }

        public BehaviorDbContext(string instance)
            : base(SqlceDbContextHelper.CreateConnection(instance, "Behaviors"), GetModel(), true)
        {
        }

        static DbCompiledModel GetModel()
        {
            return SqlceDbContextHelper.GetModel(typeof(BehaviorDbContext).Name, builder =>
            {
                builder.Entity<BehaviorRecord>();
                builder.Entity<ItemUsers>();
                builder.Entity<UserItems>();
            });
        }
    }
}