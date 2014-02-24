using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns.Events
{
    public interface IAddInMigrationEvents
    {
        void OnUpgraded(AddInMeta addIn);

        void OnDowngrading(AddInMeta addIn);
    }
}
