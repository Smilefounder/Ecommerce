using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    /// <summary>
    /// A checker to check if a promotion can be used for the current context.
    /// </summary>
    public class PromotionConditionChecker
    {
        private RuleEngine _ruleEngine;

        public PromotionConditionChecker(RuleEngine ruleEngine)
        {
            _ruleEngine = ruleEngine;
        }

        public CheckPromotionConditionResult CheckConditions(Promotion promotion, PricingContext context)
        {
            var result = new CheckPromotionConditionResult();

            if (!promotion.Conditions.Any())
            {
                result.Success = true;
                foreach (var item in context.Items)
                {
                    result.MatchedItems.Add(item);
                }

                return result;
            }

            var operators = _ruleEngine.ComparisonOperators.Select(o => o.Name).ToList();

            foreach (var item in context.Items)
            {
                var contextModel = new PromotionConditionContextModel
                {
                    Item = item,
                    Customer = context.Customer
                };

                if (_ruleEngine.CheckConditions(promotion.Conditions, contextModel))
                {
                    result.Success = true;
                    result.MatchedItems.Add(item);
                }
            }

            return result;
        }
    }
}
