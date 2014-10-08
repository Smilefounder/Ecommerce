using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Events
{
    public class SavingDbChanges : IEvent
    {
        public CommerceDbContext DbContext { get; private set; }

        public SavingDbChanges(CommerceDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
