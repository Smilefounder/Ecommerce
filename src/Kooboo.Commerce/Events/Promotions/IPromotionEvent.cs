using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    [ActivityVisible("Promotion Events")]
    public interface IPromotionEvent : IEvent
    {
        int PromotionId { get; }
    }
}
