using Kooboo.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class ConditionCheckingContext
    {
        public Promotion Promotion { get; private set; }

        public PromotionCondition Condition { get; private set; }

        public PricingContext PricingContext { get; private set; }

        public ConditionCheckingContext(Promotion promotion, PromotionCondition condition, PricingContext context)
        {
            Promotion = promotion;
            Condition = condition;
            PricingContext = context;
        }
    }
}
