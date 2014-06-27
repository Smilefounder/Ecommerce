using Kooboo.Commerce.Rules.Evaluation;
using Kooboo.Commerce.Rules.Expressions;
using Kooboo.Commerce.Rules.Operators;
using Kooboo.Commerce.Rules.Parameters;
using Kooboo.Commerce.Rules.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class RuleEngine
    {
        private ParameterProviderCollection _parameterProviders;
        private ComparisonOperatorCollection _comparisonOperators;

        public ParameterProviderCollection ParameterProviders
        {
            get
            {
                if (_parameterProviders == null)
                {
                    _parameterProviders = Kooboo.Commerce.Rules.Parameters.ParameterProviders.Providers;
                }

                return _parameterProviders;
            }
            set
            {
                _parameterProviders = value;
            }
        }

        public ComparisonOperatorCollection ComparisonOperators
        {
            get
            {
                if (_comparisonOperators == null)
                {
                    _comparisonOperators = Kooboo.Commerce.Rules.Operators.ComparisonOperators.Operators;
                }
                return _comparisonOperators;
            }
            set
            {
                _comparisonOperators = value;
            }
        }

        public RuleEngine()
        {
        }

        public bool CheckConditions(IEnumerable<Condition> conditions, object dataContext)
        {
            foreach (var condition in conditions)
            {
                if (String.IsNullOrWhiteSpace(condition.Expression))
                {
                    continue;
                }

                if (!CheckCondition(condition, dataContext))
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckCondition(Condition condition, object dataContext)
        {
            // Empty condition always pass
            if (String.IsNullOrWhiteSpace(condition.Expression))
            {
                return true;
            }

            if (condition.Type == ConditionType.Include)
            {
                return CheckCondition(condition.Expression, dataContext);
            }
            else
            {
                return CheckCondition(condition.Expression, dataContext) == false;
            }
        }

        /// <summary>
        /// Check if the specified condition can be fullfilled by the context.
        /// </summary>
        /// <param name="expression">The condition expression. e.g., Param1 > 18 AND Param2 == "Value"</param>
        /// <param name="dataContext">The contextual object.</param>
        /// <returns>True if the condtion can pass, otherwise false.</returns>
        public bool CheckCondition(string expression, object dataContext)
        {
            Require.NotNullOrEmpty(expression, "expression");
            Require.NotNull(dataContext, "dataContext");

            return CheckCondition(Expression.Parse(expression, ComparisonOperators.NamesAndAlias), dataContext);
        }

        public bool CheckCondition(Expression expression, object dataContext)
        {
            Require.NotNull(expression, "expression");
            Require.NotNull(dataContext, "dataContext");

            var evaluator = new ConditionExpressionEvaluator(ParameterProviders, ComparisonOperators);
            return evaluator.Evaluate(expression, dataContext);
        }
    }
}
