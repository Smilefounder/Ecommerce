using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    [Event(Order = 200)]
    public class PromotionUpdated : DomainEvent, IPromotionEvent
    {
        [Reference(typeof(Promotion))]
        public int PromotionId { get; set; }

        protected PromotionUpdated() { }

        public PromotionUpdated(Promotion promotion)
        {
            PromotionId = promotion.Id;
        }
    }
}
