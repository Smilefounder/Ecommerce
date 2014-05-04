using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.ComponentModel
{
    // Note: We don't have a corresponding INotifyUpdating is because,
    //       it's very difficult to track update operations, devlopers can update a field by calling the property setter.
    //       Besides, pure CRUD events are not so useful. 
    //       We need more "Domain Events" which capture the business rules instead of CRUD events.
    //       So INotifyUpdated is also supposed to be used as less as possible.
    public interface INotifyUpdated
    {
        void NotifyUpdated();
    }
}
