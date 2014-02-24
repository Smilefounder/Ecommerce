using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class ConditionCheckingResult
    {
        public bool Success { get; private set; }

        /// <summary>
        /// Ordered items matched by this condition. Matched items can be used by promotion policy when nessisary.
        /// </summary>
        public IList<PricingItem> MatchedItems { get; set; }

        public ConditionCheckingResult(bool success)
        {
            Success = success;
            MatchedItems = new List<PricingItem>();
        }

        public static ConditionCheckingResult Failed()
        {
            return new ConditionCheckingResult(false);
        }
    }
}
