using Kooboo.Commerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Promotions
{
    public class PromotionUpdated : Event, IPromotionEvent
    {
        public Promotion Promotion { get; private set; }

        public PromotionUpdated(Promotion promotion)
        {
            Promotion = promotion;
        }
    }
}
