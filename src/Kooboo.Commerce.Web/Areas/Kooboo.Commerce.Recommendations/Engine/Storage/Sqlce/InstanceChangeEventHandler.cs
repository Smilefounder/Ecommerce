using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce
{
    class InstanceChangeEventHandler : IHandle<CommerceInstanceCreated>, IHandle<CommerceInstanceDeleted>
    {
        public void Handle(CommerceInstanceCreated @event)
        {
            SqlceRecommendationEngineConfiguration.Initialize(@event.InstanceName);
        }

        public void Handle(CommerceInstanceDeleted @event)
        {
            SqlceRecommendationEngineConfiguration.Dispose(@event.InstanceName);
        }
    }
}