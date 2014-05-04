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
    /// Class to evaluate the result of a condition expression.
    /// </summary>
    public class ConditionExpressionEvaluator : ExpressionVisitor
    {
        private object _contextModel;
        private Stack<bool> _results = new Stack<bool>();
        private List<IConditionParameter> _availableParameters;
        private IEnumerable<IConditionParameterProvider> _modelParameterProviders;
        private IComparisonOperatorProvider _comparisonOperatorProvider;

        public ConditionExpressionEvaluator(
            IEnumerable<IConditionParameterProvider> modelParameterProviders,
            IComparisonOperatorProvider comparisonOperatorProvider)
        {
            Require.NotNull(modelParameterProviders, "modelParameterProviders");
            Require.NotNull(comparisonOperatorProvider, "comparisonOperatorProvider");

            _modelParameterProviders = modelParameterProviders;
            _comparisonOperatorProvider = comparisonOperatorProvider;
        }

        /// <summary>
        /// Evaludates the result of the condition expression.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="contextModel">The context model.</param>
        /// <returns>True if the condition expression passes, false otherwise.</returns>
        public bool Evaluate(Expression expression, object contextModel)
        {
            Require.NotNull(expression, "expression");
            Require.NotNull(contextModel, "contextModel");

            _contextModel = contextModel;

            var contextModelType = contextModel.GetType();
            _availableParameters = _modelParameterProviders.SelectMany(x => x.GetParameters(contextModelType))
                                                           .DistinctBy(x => x.Name)
                                                           .ToList();

            Visit(expression);

            Debug.Assert(_results.Count == 1);

            return _results.Pop();
        }

        protected override void Visit(ConditionExpression exp)
        {
            var paramName = exp.Param.ParamName;
            var param = _availableParameters.FirstOrDefault(x => x.Name == paramName);
            if (param == null)
                throw new InvalidOperationException("Unrecognized parameter \"" + paramName + "\" or it's not accessable in currect context.");


            var @operator = _comparisonOperatorProvider.GetOperatorByName(exp.Operator);
            if (@operator == null)
            {
                @operator = ComparisonOperators.GetOperatorFromShortcut(exp.Operator);
            }

            if (@operator == null)
                throw new InvalidOperationException("Unrecognized comparison operator \"" + exp.Operator + "\".");

            var paramValue = param.GetValue(_contextModel);
            var conditionValue = GetConditionValue(exp.Value, param);
            var result = @operator.Apply(param, paramValue, conditionValue);

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

        private object GetConditionValue(ConditionValueExpression exp, IConditionParameter param)
        {
            return param.ParseValue(exp.Value);
        }
    }
}
