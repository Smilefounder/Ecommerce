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

        public BehaviorDbContext(string instance, string behaviorType)
            : base(SqlceDbContextHelper.CreateConnection(instance, "Behaviors"), GetModel(instance, "Behaviors"), true)
        {
        }

        static DbCompiledModel GetModel(string instance, string dbName)
        {
            return SqlceDbContextHelper.GetModel(instance, dbName, builder =>
            {
                builder.Entity<BehaviorRecord>();
            });
        }
    }
}