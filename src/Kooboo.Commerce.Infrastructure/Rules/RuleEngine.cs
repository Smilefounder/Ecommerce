using Kooboo.Commerce.Rules.Expressions;
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
        private ParameterProviderManager _parameterProviderManager;
        private ComparisonOperatorManager _comparisonOperatorManager;

        public ParameterProviderManager ParameterProviderManager
        {
            get
            {
                return _parameterProviderManager;
            }
        }

        public ComparisonOperatorManager ComparisonOperatorManager
        {
            get
            {
                return _comparisonOperatorManager;
            }
        }

        public RuleEngine()
            : this(ParameterProviderManager.Instance, ComparisonOperatorManager.Instance)
        {
        }

        public RuleEngine(ParameterProviderManager parameterProviderManager, ComparisonOperatorManager comparisonOperatorManager)
        {
            Require.NotNull(parameterProviderManager, "parameterProviderManager");
            Require.NotNull(comparisonOperatorManager, "operatorManager");

            _parameterProviderManager = parameterProviderManager;
            _comparisonOperatorManager = comparisonOperatorManager;
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

            return CheckCondition(Expression.Parse(expression, _comparisonOperatorManager.AllOperatorNamesAndAlias()), dataContext);
        }

        public bool CheckCondition(Expression expression, object dataContext)
        {
            Require.NotNull(expression, "expression");
            Require.NotNull(dataContext, "dataContext");

            var evaluator = new ConditionExpressionEvaluator(_parameterProviderManager, _comparisonOperatorManager);
            return evaluator.Evaluate(expression, dataContext);
        }
    }
}
