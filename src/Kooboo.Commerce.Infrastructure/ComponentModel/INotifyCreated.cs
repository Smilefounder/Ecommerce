using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.ComponentModel
{
    // Note: We don't have a corresponding INotifyCreating is because,
    //       we can't directly save the model in the event message because event messages need to be serializable.
    //       And before the entity is saved, we are not able to get its identity, 
    //       so I think it's not so useful to expose an INotifyCreting, because developers can't do much useful things with the entity.
    public interface INotifyCreated
    {
        void NotifyCreated();
    }
}
