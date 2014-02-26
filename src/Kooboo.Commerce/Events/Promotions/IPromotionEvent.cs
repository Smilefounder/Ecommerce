using Kooboo.Commerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    [EventCategory("Promotion Events")]
    public interface IPromotionEvent : IDomainEvent
    {
        Promotion Promotion { get; }
    }
}
