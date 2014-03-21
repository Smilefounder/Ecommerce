using Kooboo.CMS.Common.Runtime;
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
        private IModelParameterProvider _parameterProvider;
        private IComparisonOperatorProvider _comparisonOperatorProvider;

        public RuleEngine()
            : this(EngineContext.Current.Resolve<IModelParameterProvider>()
                , EngineContext.Current.Resolve<IComparisonOperatorProvider>())
        {
        }

        public RuleEngine(
            IModelParameterProvider parameterProvider,
            IComparisonOperatorProvider operatorProvider)
        {
            Require.NotNull(parameterProvider, "parameterProvider");
            Require.NotNull(operatorProvider, "operatorProvider");

            _parameterProvider = parameterProvider;
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

            var evaluator = new ConditionExpressionEvaluator(_parameterProvider, _comparisonOperatorProvider);
            return evaluator.Evaluate(expression, contextModel);
        }
    }
}
