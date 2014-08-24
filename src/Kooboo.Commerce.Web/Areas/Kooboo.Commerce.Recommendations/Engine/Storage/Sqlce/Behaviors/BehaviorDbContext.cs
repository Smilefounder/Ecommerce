using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            : base(SqlceDbContextHelper.CreateConnection(instance, "Behaviors"), SqlceDbContextHelper.GetModel("BehaviorDbContext"), true)
        {
        }
    }
}