using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Events
{
    public class DbContextSavingChanges : Event
    {
        public CommerceDbContext DbContext { get; private set; }

        public DbContextSavingChanges(CommerceDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
