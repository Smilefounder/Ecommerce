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
        private IConditionParameterFactory _parameterFactory;
        private IComparisonOperatorFactory _comparisonOperatorFactory;
        private IDataSourceFactory _dataSourceFactory;

        public RuleEngine()
            : this(new DefaultConditionParameterFactory()
                , new DefaultComparisonOperatorFactory()
                , new DefaultDataSourceFactory())
        {
        }

        public RuleEngine(
            IConditionParameterFactory parameterFactory,
            IComparisonOperatorFactory operatorFactory,
            IDataSourceFactory dataSourceFactory)
        {
            Require.NotNull(parameterFactory, "parameterFactory");
            Require.NotNull(operatorFactory, "operatorFactory");
            Require.NotNull(dataSourceFactory, "dataSourceFactory");

            _parameterFactory = parameterFactory;
            _comparisonOperatorFactory = operatorFactory;
            _dataSourceFactory = dataSourceFactory;
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

            return new ConditionExpressionEvaluator(
                _parameterFactory
                , _comparisonOperatorFactory
                , _dataSourceFactory)
                .Evaluate(Expression.Parse(conditionExpression), contextModel);
        }
    }
}
