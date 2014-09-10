using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    public class PromotionEnabled : Event, IPromotionEvent
    {
        [Reference(typeof(Promotion))]
        public int PromotionId { get; set; }

        protected PromotionEnabled() { }

        public PromotionEnabled(Promotion promotion)
        {
            PromotionId = promotion.Id;
        }
    }
}
