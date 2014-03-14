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

            return new RuleChecker(this).Check(Expression.Parse(conditionExpression), contextModel);
        }

        class RuleChecker : ExpressionVisitor
        {
            private RuleEngine _engine;
            private object _contextModel;
            private Stack<bool> _results = new Stack<bool>();
            private List<ModelParameter> _availableParameters;

            public RuleChecker(RuleEngine engine)
            {
                _engine = engine;
            }

            public bool Check(Expression expression, object contextModel)
            {
                _contextModel = contextModel;
                _availableParameters = new ContextModelInspector(_engine._parameterFactory)
                                                .GetAvailableParameters(contextModel.GetType())
                                                .ToList();

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

                var param = modelParam.Parameter;
                object paramContainerModel = _contextModel;

                if (modelParam.Path is PropertyInfo)
                {
                    paramContainerModel = ((PropertyInfo)modelParam.Path).GetValue(_contextModel, null);
                }
                if (modelParam.Path is FieldInfo)
                {
                    paramContainerModel = ((FieldInfo)modelParam.Path).GetValue(_contextModel);
                }

                var @operator = _engine._comparisonOperatorFactory.FindByName(exp.Operator);
                if (@operator == null)
                {
                    @operator = ComparisonOperators.GetOperatorFromShortcut(exp.Operator);
                }

                if (@operator == null)
                    throw new InvalidOperationException("Unrecognized comparison operator \"" + exp.Operator + "\".");

                var conditionValue = GetConditionValue(exp.Value, param);
                var result = @operator.Apply(param, param.GetValue(paramContainerModel), conditionValue);

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
                    var dataSource = _engine._dataSourceFactory.FindById(exp.DataSourceId);
                    if (dataSource == null)
                        throw new InvalidOperationException("Cannot find data source with id: " + exp.DataSourceId + ".");

                    var item = dataSource.GetItems(param)
                                         .FirstOrDefault(x => x.Value.Equals(exp.Value));

                    if (item == null)
                        throw new InvalidOperationException("Cannot find value \"" + exp.Value + "\" in data source \"" + exp.DataSourceId + "\".");

                    value = item.Value;
                }
                else
                {
                    value = exp.Value;
                }

                return Convert.ChangeType(value, param.ValueType.ToClrType());
            }
        }
    }
}
