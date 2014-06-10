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
        private IEnumerable<IParameterProvider> _modelParameterProviders;
        private IComparisonOperatorProvider _comparisonOperatorProvider;

        public RuleEngine()
            : this(EngineContext.Current.ResolveAll<IParameterProvider>(), EngineContext.Current.Resolve<IComparisonOperatorProvider>())
        {
        }

        public RuleEngine(IEnumerable<IParameterProvider> parameterProviders, IComparisonOperatorProvider operatorProvider)
        {
            Require.NotNull(parameterProviders, "modelParameterProviders");
            Require.NotNull(operatorProvider, "operatorProvider");

            _modelParameterProviders = parameterProviders;
            _comparisonOperatorProvider = operatorProvider;
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

            return CheckCondition(Expression.Parse(expression, _comparisonOperatorProvider.GetAllOperators().Select(o => o.Name).ToList()), dataContext);
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
