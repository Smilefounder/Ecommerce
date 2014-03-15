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
    class OnActivityDetached : IHandles<ActivityDetached>
    {
        private IRepository<AttachedActivity> _repository;

        public OnActivityDetached(IRepository<AttachedActivity> repository)
        {
            _repository = repository;
        }

        public void Handle(ActivityDetached @event, Commerce.Events.Dispatching.EventDispatchingContext context)
        {
            _repository.Delete(@event.Activity);
        }
    }
}
