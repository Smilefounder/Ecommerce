using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Shipping.ByWeight.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight.Data
{
    [Dependency]
    public class AddInDbContext : DbContext
    {
        private DbSet<ByWeightShippingRule> _byWeightShippingRules;

        public IDbSet<ByWeightShippingRule> ByWeightShippingRules
        {
            get
            {
                if (_byWeightShippingRules == null)
                {
                    _byWeightShippingRules = Set<ByWeightShippingRule>();
                }

                return _byWeightShippingRules;
            }
        }

        public AddInDbContext()
            : this("Kooboo.Commerce") { }

        public AddInDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(AddInDbContext).Assembly);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}