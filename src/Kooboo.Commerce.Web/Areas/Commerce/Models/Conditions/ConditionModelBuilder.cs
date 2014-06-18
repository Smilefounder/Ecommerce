using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions
{
    // In the UI, the first level is several conditions, they have the "AND" relationship.
    // Then for each condition, we have several "AND" groups.
    // And then for each "AND" group, we have several "OR" comparisons.
    // What we do here is to transform the generic expression tree into this specifical UI expression form.
    // 
    // Condition 1: [Include/Exclude]
    // ------------------------------
    //      Param1 == Value1
    //      OR
    //      Param2 == Value2
    // AND
    //      Param3 == Value3
    // AND
    //      Param4 == Value4
    //      OR
    //      Param5 == Value5
    public class ConditionModelBuilder : ExpressionVisitor
    {
        private ComparisonOperatorManager _operatorManager = ComparisonOperatorManager.Instance;
        private ParameterProviderManager _parameterProviderManager = ParameterProviderManager.Instance;
        private List<ConditionParameter> _parameters;
        private Stack<object> _stack = new Stack<object>();

        public ConditionModel Build(string expression, Type dataContextType, ConditionType conditionType)
        {
            if (String.IsNullOrWhiteSpace(expression))
            {
                return new ConditionModel();
            }

            _stack = new Stack<object>();
            _parameters = _parameterProviderManager.Providers
                                                   .SelectMany(x => x.GetParameters(dataContextType).ToList())
                                                   .DistinctBy(x => x.Name)
                                                   .ToList();

            var exp = Expression.Parse(expression);
            Visit(exp);

            var top = _stack.Peek();
            if (top is ComparisonModel)
            {
                BuildComparisonGroup();
            }

            BuildConditionModel(conditionType);

            var model = (ConditionModel)_stack.Pop();
            model.Expression = expression;

            return model;
        }

        protected override void Visit(LogicalBindaryExpression exp)
        {
            // OR is the 3rd level
            if (exp.Operator == LogicalOperator.OR)
            {
                // Simply visit the 3rd OR level and build all ComparisonModels
                Visit(exp.Left);
                Visit(exp.Right);
            }
            if (exp.Operator == LogicalOperator.AND)
            {
                // Because this class only build for ONE condition at a time.
                // So if we encounter a AND expression, it must be the 2nd level, that is, the (ComparisonGroup AND ComparisonGroup) level
                Visit(exp.Left);
                BuildComparisonGroup();

                Visit(exp.Right);
                BuildComparisonGroup();
            }
        }

        private void BuildConditionModel(ConditionType conditionType)
        {
            var model = new ConditionModel
            {
                Type = conditionType
            };

            while (_stack.Count > 0)
            {
                model.Groups.Add((ComparisonGroup)_stack.Pop());
            }

            model.Groups.Reverse();

            _stack.Push(model);
        }

        private void BuildComparisonGroup()
        {
            var comparisons = new List<ComparisonModel>();
            while (_stack.Count > 0 && (_stack.Peek() is ComparisonModel))
            {
                comparisons.Add((ComparisonModel)_stack.Pop());
            }

            comparisons.Reverse();

            if (comparisons.Count > 0)
            {
                var group = new ComparisonGroup
                {
                    Comparisons = comparisons
                };
                _stack.Push(group);
            }
        }

        // Comparison is the most nested level
        protected override void Visit(ComparisonExpression exp)
        {
            var model = new ComparisonModel
            {
                ParamName = exp.Param.ParamName,
                Value = exp.Value.Value,
                Operator = exp.Operator
            };

            var op = _operatorManager.Find(exp.Operator);
            if (op != null)
            {
                model.OperatorDisplayName = op.Name;
                if (!String.IsNullOrWhiteSpace(op.Alias))
                {
                    model.OperatorDisplayName = op.Alias;
                }
            }

            var param = _parameters.Find(x => x.Name.Equals(model.ParamName, StringComparison.OrdinalIgnoreCase));
            if (param != null)
            {
                model.ValueType = param.ValueType.FullName;
                model.IsNumberValue = param.ValueType.IsNumber();
            }

            _stack.Push(model);
        }
    }
}