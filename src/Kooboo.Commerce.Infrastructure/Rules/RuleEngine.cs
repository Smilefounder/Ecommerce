using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
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
    [Dependency(typeof(RuleEngine))]
    public class RuleEngine
    {
        private IEnumerable<IConditionParameterProvider> _modelParameterProviders;
        private IComparisonOperatorProvider _comparisonOperatorProvider;

        public RuleEngine(IEnumerable<IConditionParameterProvider> modelParameterProviders, IComparisonOperatorProvider operatorProvider)
        {
            Require.NotNull(modelParameterProviders, "modelParameterProviders");
            Require.NotNull(operatorProvider, "operatorProvider");

            _modelParameterProviders = modelParameterProviders;
            _comparisonOperatorProvider = operatorProvider;
        }

        /// <summary>
        /// Check if the specified condition can be fullfilled by the context.
        /// </summary>
        /// <param name="conditionExpression">The condition expression. e.g., Param1 > 18 AND Param2 == "Value"</param>
        /// <param name="contextModel">The contextual model.</param>
        /// <returns>True if the condtion can pass, otherwise false.</returns>
        public bool CheckCondition(string conditionExpression, object contextModel)
        {
            Require.NotNullOrEmpty(conditionExpression, "conditionExpression");
            Require.NotNull(contextModel, "contextModel");

            return CheckCondition(Expression.Parse(conditionExpression), contextModel);
        }

        public bool CheckCondition(Expression expression, object contextModel)
        {
            Require.NotNull(expression, "expression");
            Require.NotNull(contextModel, "contextModel");

            var evaluator = new ConditionExpressionEvaluator(_modelParameterProviders, _comparisonOperatorProvider);
            return evaluator.Evaluate(expression, contextModel);
        }
    }
}
