using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    [Serializable]
    public class PromotionEnabled : DomainEvent, IPromotionEvent
    {
        [ConditionParameter]
        public int PromotionId { get; set; }

        [ConditionParameter]
        public string PromotionName { get; set; }

        public PromotionEnabled() { }

        public PromotionEnabled(Promotion promotion)
        {
            PromotionId = promotion.Id;
            PromotionName = promotion.Name;
        }
    }
}
