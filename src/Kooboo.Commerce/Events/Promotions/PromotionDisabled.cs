using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    [Event(Order = 400)]
    public class PromotionDisabled : BusinessEvent, IPromotionEvent
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
