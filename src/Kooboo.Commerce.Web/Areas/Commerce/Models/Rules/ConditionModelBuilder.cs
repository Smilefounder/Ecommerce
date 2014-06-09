using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class ConditionModelBuilder : ExpressionVisitor
    {
        private IEnumerable<IParameterProvider> _parameterProviders;
        private List<ConditionParameter> _parameters;
        private Stack<List<ConditionModel>> _conditionTrees = new Stack<List<ConditionModel>>();

        public ConditionModelBuilder(IEnumerable<IParameterProvider> parameterProviders)
        {
            Require.NotNull(parameterProviders, "parameterProviders");
            _parameterProviders = parameterProviders;
        }

        public IList<ConditionModel> BuildFrom(string expression, Type contextModelType)
        {
            if (String.IsNullOrWhiteSpace(expression))
            {
                return new List<ConditionModel>();
            }

            if (contextModelType == null)
                throw new ArgumentNullException("contextModelType");

            _parameters = _parameterProviders.SelectMany(x => x.GetParameters(contextModelType).ToList())
                                             .DistinctBy(x => x.Name)
                                             .ToList();

            Visit(Expression.Parse(expression));

            return _conditionTrees.Pop();
        }

        protected override void Visit(LogicalBindaryExpression exp)
        {
            Visit(exp.Left);
            Visit(exp.Right);

            var rightTreeConditions = _conditionTrees.Pop();
            var leftTreeConditions = _conditionTrees.Pop();

            var conditions = new List<ConditionModel>();
            conditions.AddRange(leftTreeConditions);

            if (leftTreeConditions.Count > 1)
            {
                var prevOperator = leftTreeConditions[leftTreeConditions.Count - 1].LogicalOperator;

                if (prevOperator == ConditionLogicalOperator.OR && exp.Operator == LogicalOperator.AND)
                {
                    rightTreeConditions[0].LogicalOperator = ConditionLogicalOperator.ThenAND;
                }
                else
                {
                    rightTreeConditions[0].LogicalOperator = SimplyConvertFrom(exp.Operator);
                }
            }
            else
            {
                rightTreeConditions[0].LogicalOperator = SimplyConvertFrom(exp.Operator);
            }

            conditions.AddRange(rightTreeConditions);

            _conditionTrees.Push(conditions);
        }

        private ConditionLogicalOperator SimplyConvertFrom(LogicalOperator @operator)
        {
            if (@operator == LogicalOperator.AND)
            {
                return ConditionLogicalOperator.AND;
            }

            return ConditionLogicalOperator.OR;
        }

        protected override void Visit(ComparisonExpression exp)
        {
            var model = new ConditionModel
            {
                ParamName = exp.Param.ParamName,
                Value = exp.Value.Value,
                ComparisonOperator = exp.Operator
            };

            var @operator = ComparisonOperators.GetOperatorFromShortcut(exp.Operator);
            if (@operator != null)
            {
                model.ComparisonOperator = @operator.Name;
            }

            var param = _parameters.Find(x => x.Name.Equals(model.ParamName, StringComparison.OrdinalIgnoreCase));
            if (param == null)
                throw new InvalidOperationException("Parameter \"" + model.ParamName + "\" is invalid or not available in current context.");

            model.ValueType = param.ValueType.FullName;
            model.IsNumberValue = param.ValueType.IsNumber();

            _conditionTrees.Push(new List<ConditionModel> { model });
        }
    }
}