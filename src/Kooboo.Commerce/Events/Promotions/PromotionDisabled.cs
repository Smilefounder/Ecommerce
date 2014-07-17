using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    [ActivityEvent(Order = 200)]
    public class PromotionDisabled : Event, IPromotionEvent
    {
        [Reference(typeof(Promotion))]
        public int PromotionId { get; set; }

        protected PromotionDisabled() { }

        public PromotionDisabled(Promotion promotion)
        {
            PromotionId = promotion.Id;
        }
    }
}
