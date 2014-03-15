using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class ConditionExpressionEvaluator : ExpressionVisitor
    {
        private object _contextModel;
        private Stack<bool> _results = new Stack<bool>();
        private List<ConditionParameterInfo> _availableParameters;
        private IConditionParameterFactory _parameterFactory;
        private IComparisonOperatorFactory _comparisonOperatorFactory;
        private IParameterValueSourceFactory _dataSourceFactory;

        public ConditionExpressionEvaluator(
            IConditionParameterFactory parameterFactory,
            IComparisonOperatorFactory comparisonOperatorFactory,
            IParameterValueSourceFactory dataSourceFactory)
        {
            _parameterFactory = parameterFactory;
            _comparisonOperatorFactory = comparisonOperatorFactory;
            _dataSourceFactory = dataSourceFactory;
        }

        public bool Evaluate(Expression expression, object contextModel)
        {
            _contextModel = contextModel;
            _availableParameters = _parameterFactory.GetConditionParameterInfos(contextModel.GetType()).ToList();

            Visit(expression);

            Debug.Assert(_results.Count == 1);

            return _results.Pop();
        }

        protected override void Visit(ConditionExpression exp)
        {
            var paramName = exp.Param.ParamName;
            var modelParam = _availableParameters.FirstOrDefault(x => x.Parameter.Name == paramName);
            if (modelParam == null)
                throw new InvalidOperationException("Unrecognized parameter \"" + paramName + "\" or it's not accessable in currect context.");


            var @operator = _comparisonOperatorFactory.FindByName(exp.Operator);
            if (@operator == null)
            {
                @operator = ComparisonOperators.GetOperatorFromShortcut(exp.Operator);
            }

            if (@operator == null)
                throw new InvalidOperationException("Unrecognized comparison operator \"" + exp.Operator + "\".");

            var param = modelParam.Parameter;
            var paramValue = modelParam.GetValue(_contextModel);
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
            string value = null;

            if (!String.IsNullOrEmpty(exp.DataSourceId))
            {
                var dataSource = _dataSourceFactory.FindById(exp.DataSourceId);
                if (dataSource == null)
                    throw new InvalidOperationException("Cannot find data source with id: " + exp.DataSourceId + ".");

                var item = dataSource.GetValues(param)
                                     .FirstOrDefault(x => x.Value.Equals(exp.Value));

                if (item == null)
                    throw new InvalidOperationException("Cannot find value \"" + exp.Value + "\" in data source \"" + exp.DataSourceId + "\".");

                value = item.Value;
            }
            else
            {
                value = exp.Value;
            }

            return param.ParseValue(value);
        }
    }
}
