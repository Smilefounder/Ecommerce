using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    [Serializable]
    [Event(Category = EventCategories.Promotions, Order = 400)]
    public class PromotionDeleted : DomainEvent, IPromotionEvent
    {
        [Param]
        public int PromotionId { get; set; }

        [Param]
        public string PromotionName { get; set; }

        public PromotionDeleted() { }

        public PromotionDeleted(Promotion promotion)
        {
            PromotionId = promotion.Id;
            PromotionName = promotion.Name;
        }
    }
}
