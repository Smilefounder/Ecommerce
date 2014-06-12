using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    [Category("Promotions", Order = 500)]
    public interface IPromotionEvent : IBusinessEvent
    {
        int PromotionId { get; }
    }
}
