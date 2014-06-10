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
        private IComparisonOperatorProvider _comparisonOperatorProvider;

        public PromotionConditionChecker(RuleEngine ruleEngine, IComparisonOperatorProvider comparisonOperatorProvider)
        {
            _ruleEngine = ruleEngine;
            _comparisonOperatorProvider = comparisonOperatorProvider;
        }

        public CheckPromotionConditionResult CheckConditions(Promotion promotion, PricingContext context)
        {
            var result = new CheckPromotionConditionResult();

            if (String.IsNullOrWhiteSpace(promotion.ConditionsExpression))
            {
                result.Success = true;
                foreach (var item in context.Items)
                {
                    result.MatchedItems.Add(item);
                }

                return result;
            }

            var operators = _comparisonOperatorProvider.GetAllOperators().Select(o => o.Name).ToList();
            var expression = Expression.Parse(promotion.ConditionsExpression, operators);

            foreach (var item in context.Items)
            {
                var contextModel = new PromotionConditionContextModel
                {
                    Item = item,
                    Customer = context.Customer
                };

                if (_ruleEngine.CheckCondition(expression, contextModel))
                {
                    result.Success = true;
                    result.MatchedItems.Add(item);
                }
            }

            return result;
        }
    }
}
