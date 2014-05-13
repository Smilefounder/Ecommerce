using Kooboo.Commerce.Activities.Events;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities.Handlers
{
    // NOTE: Entity Framwork doesn't support delete-orphan operation.
    // So we delete the orphan entities here to keep the domain model clean.
    // It's a hack but I haven't find more elegant solution at the moment.
    class OnActivityDetached : IHandle<ActivityDetached>
    {
        private IRepository<AttachedActivityInfo> _repository;

        public OnActivityDetached(IRepository<AttachedActivityInfo> repository)
        {
            _repository = repository;
        }

        public void Handle(ActivityDetached @event)
        {
            _repository.Delete(@event.Activity);
        }
    }
}
