using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    [Serializable]
    [Event(Order = 100)]
    public class PromotionCreated : DomainEvent, IPromotionEvent
    {
        [Reference(typeof(Promotion))]
        public int PromotionId { get; set; }

        public string PromotionName { get; set; }

        public PromotionCreated() { }

        public PromotionCreated(Promotion promotion)
        {
            PromotionId = promotion.Id;
            PromotionName = promotion.Name;
        }
    }
}
