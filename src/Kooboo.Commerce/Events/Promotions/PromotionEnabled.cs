using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    [Serializable]
    [Event(Category = EventCategories.Promotions, Order = 200)]
    public class PromotionEnabled : DomainEvent, IPromotionEvent
    {
        [Reference(typeof(Promotion))]
        public int PromotionId { get; set; }

        public string PromotionName { get; set; }

        public PromotionEnabled() { }

        public PromotionEnabled(Promotion promotion)
        {
            PromotionId = promotion.Id;
            PromotionName = promotion.Name;
        }
    }
}
