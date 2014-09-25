using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations
{
    class InstanceEventHandler : IHandle<CommerceInstanceCreated>, IHandle<CommerceInstanceDeleted>
    {
        public void Handle(CommerceInstanceCreated @event, CommerceInstance instance)
        {
            RecommendationEngineConfiguration.Initialize(@event.InstanceName);
        }

        public void Handle(CommerceInstanceDeleted @event, CommerceInstance instance)
        {
            RecommendationEngineConfiguration.Dispose(@event.InstanceName);
        }
    }
}