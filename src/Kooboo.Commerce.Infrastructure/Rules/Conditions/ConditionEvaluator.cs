using Kooboo.Commerce.Rules.Conditions.Expressions;
using Kooboo.Commerce.Rules.Conditions.Operators;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions
{
    public class ConditionEvaluator
    {
        private RuleParameterProviderCollection _parameterProviders;
        private ComparisonOperatorCollection _comparisonOperators;

        public RuleParameterProviderCollection ParameterProviders
        {
            get
            {
                if (_parameterProviders == null)
                {
                    _parameterProviders = Kooboo.Commerce.Rules.Parameters.RuleParameterProviders.Providers;
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
                    _comparisonOperators = Kooboo.Commerce.Rules.Conditions.Operators.ComparisonOperators.Operators;
                }
                return _comparisonOperators;
            }
            set
            {
                _comparisonOperators = value;
            }
        }

        public bool Evaluate(IEnumerable<Condition> conditions, object dataContext)
        {
            foreach (var condition in conditions)
            {
                if (String.IsNullOrWhiteSpace(condition.Expression))
                {
                    continue;
                }

                if (!Evaluate(condition, dataContext))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Evaluate(Condition condition, object dataContext)
        {
            // Empty condition always pass
            if (String.IsNullOrWhiteSpace(condition.Expression))
            {
                return true;
            }

            if (condition.Type == ConditionType.Include)
            {
                return Evaluate(condition.Expression, dataContext);
            }
            else
            {
                return Evaluate(condition.Expression, dataContext) == false;
            }
        }

        /// <summary>
        /// Check if the specified condition can be fullfilled by the context.
        /// </summary>
        /// <param name="expression">The condition expression. e.g., Param1 > 18 AND Param2 == "Value"</param>
        /// <param name="dataContext">The contextual object.</param>
        /// <returns>True if the condtion can pass, otherwise false.</returns>
        public bool Evaluate(string expression, object dataContext)
        {
            Require.NotNullOrEmpty(expression, "expression");
            Require.NotNull(dataContext, "dataContext");

            return Evaluate(Expression.Parse(expression, ComparisonOperators.NamesAndAlias), dataContext);
        }

        public bool Evaluate(Expression expression, object dataContext)
        {
            Require.NotNull(expression, "expression");
            Require.NotNull(dataContext, "dataContext");

            var evaluator = new ExpressionEvaluator(ParameterProviders, ComparisonOperators);
            return evaluator.Evaluate(expression, dataContext);
        }
    }
}
