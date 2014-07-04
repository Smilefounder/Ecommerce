using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Conditions;
using Kooboo.Commerce.Rules.Conditions.Expressions;
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
        private ConditionEvaluator _ruleEngine;

        public PromotionConditionChecker(ConditionEvaluator ruleEngine)
        {
            _ruleEngine = ruleEngine;
        }

        public CheckPromotionConditionResult CheckConditions(Promotion promotion, PriceCalculationContext context)
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

                if (_ruleEngine.Evaluate(promotion.Conditions, contextModel))
                {
                    result.Success = true;
                    result.MatchedItems.Add(item);
                }
            }

            return result;
        }
    }
}
