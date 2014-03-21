using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class PromotionPolicyExecutionContext
    {
        public Promotion Promotion { get; private set; }

        public ICollection<ShoppingCartItem> ConditionMatchedItems { get; private set; }

        public PromotionPolicyExecutionContext(
            Promotion promotion,
            IEnumerable<ShoppingCartItem> conditionMatchedItems)
        {
            Require.NotNull(promotion, "promotion");

            Promotion = promotion;
            ConditionMatchedItems = (conditionMatchedItems ?? Enumerable.Empty<ShoppingCartItem>()).ToList();
        }
    }
}
