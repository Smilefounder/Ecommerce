using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// Represents a evaluator to evaluate the result of a condition expression.
    /// </summary>
    public class ConditionExpressionEvaluator : ExpressionVisitor
    {
        private object _dataContext;
        private Stack<bool> _results = new Stack<bool>();
        private List<ConditionParameter> _availableParameters;
        private ParameterProviderManager _parameterProviderManager;
        private ComparisonOperatorManager _comparisonOperatorManager;

        public ConditionExpressionEvaluator(
            ParameterProviderManager parameterProviderManager, ComparisonOperatorManager comparisonOperatorManager)
        {
            Require.NotNull(parameterProviderManager, "_parameterProviderManager");
            Require.NotNull(comparisonOperatorManager, "comparisonOperatorManager");

            _parameterProviderManager = parameterProviderManager;
            _comparisonOperatorManager = comparisonOperatorManager;
        }

        /// <summary>
        /// Evalute the value of the specified condition expression.
        /// </summary>
        /// <param name="expression">The condition expression to evaluate.</param>
        /// <param name="dataContext">The context object.</param>
        /// <returns>True if the the condition passed, false otherwise.</returns>
        public bool Evaluate(Expression expression, object dataContext)
        {
            Require.NotNull(expression, "expression");
            Require.NotNull(dataContext, "dataContext");

            _dataContext = dataContext;

            var dataContextType = dataContext.GetType();
            _availableParameters = _parameterProviderManager.Providers
                                                            .SelectMany(x => x.GetParameters(dataContextType))
                                                            .DistinctBy(x => x.Name)
                                                            .ToList();

            Visit(expression);

            Debug.Assert(_results.Count == 1);

            return _results.Pop();
        }

        protected override void Visit(ComparisonExpression exp)
        {
            var paramName = exp.Param.ParamName;
            var param = _availableParameters.FirstOrDefault(x => x.Name == paramName);
            if (param == null)
                throw new InvalidOperationException("Unrecognized parameter \"" + paramName + "\" or it's not accessable in currect context.");

            var @operator = _comparisonOperatorManager.Find(exp.Operator);
            if (@operator == null)
            {
                @operator = ComparisonOperators.GetOperatorFromShortcut(exp.Operator);
            }

            if (@operator == null)
                throw new InvalidOperationException("Unrecognized comparison operator \"" + exp.Operator + "\".");

            var result = false;

            var paramValue = param.ValueResolver.ResolveValue(param, _dataContext);
            if (paramValue != null)
            {
                var paramType = paramValue.GetType();
                object conditionValue = null;

                if (paramType.IsEnum)
                {
                    conditionValue = Enum.Parse(paramType, exp.Value.Value);
                }
                else
                {
                    conditionValue = Convert.ChangeType(exp.Value.Value, paramType);
                }

                result = @operator.Apply(param, paramValue, conditionValue);
            }

            _results.Push(result);
        }

        protected override void Visit(LogicalBindaryExpression exp)
        {
            Visit(exp.Left);

            // Check if we can get result by only checking left expression.
            // If yes, then the result in the stack is alreay the result.
            var leftResult = _results.Peek();

            // A and B, if A is false, the result will always be false, no need to check B
            if (exp.Operator == LogicalOperator.AND && leftResult == false)
            {
                return;
            }
            // A or B, if A is true, the result will always be true, no need to check B
            if (exp.Operator == LogicalOperator.OR && leftResult == true)
            {
                return;
            }

            // Pop the result of the left expression
            _results.Pop();

            // If we cannot get result by checking left expression, 
            // then check right expression.
            // The final result is same as the result of the right expression.
            // For example,
            // A and B, if A is true, then final result = B
            // A or B, if A is false, then final result = B
            Visit(exp.Right);
        }
    }
}
