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
        private IConditionParameterFactory _parameterFactory;
        private Stack<ConditionModel> _conditions = new Stack<ConditionModel>();

        public ConditionModelBuilder(IConditionParameterFactory parameterFactory)
        {
            Require.NotNull(parameterFactory, "parameterFactory");
            _parameterFactory = parameterFactory;
        }

        public IList<ConditionModel> BuildFrom(string expression)
        {
            if (String.IsNullOrWhiteSpace(expression))
            {
                return new List<ConditionModel>();
            }

            Visit(Expression.Parse(expression));

            var result = _conditions.Pop();
            if (result.IsGroup)
            {
                return result.Conditions;
            }

            return new List<ConditionModel> { result };
        }

        protected override void Visit(LogicalBindaryExpression exp)
        {
            Visit(exp.Left);
            Visit(exp.Right);

            var rightTree = _conditions.Pop();
            var leftTree = _conditions.Pop();

            rightTree.LogicalOperator = exp.Operator;

            var group = new ConditionModel
            {
                IsGroup = true,
                LogicalOperator = exp.Operator
            };

            // A group is rendered as a block wrapped with a pair of parenthese.
            // Before building a conditon group, we need to check if we can trim the redundant parenthese for left and right trees.
            // For example, the parsed expression tree might be: (A AND B) AND C
            // In the UI, user will see (A AND B) as a group, so he will see two level conditions, which is not friendly
            // In this case, we shoud transform the expression to A AND B AND C and show only one level in the UI.
            // 
            // But if the expression is A OR (B AND C),
            // we will leave B AND C as a group to make the ui easier to understand although the parenthses are also redundant.

            // Try trim redundant parenthese for left tree
            foreach (var condition in TryTrimRedundantParentheseForLeftTree(leftTree, exp.Operator))
            {
                group.Conditions.Add(condition);
            }

            // No need to trim parentheses for right tree.
            // It should all be kept in this stage.
            group.Conditions.Add(rightTree);

            _conditions.Push(group);
        }

        private List<ConditionModel> TryTrimRedundantParentheseForLeftTree(ConditionModel leftTree, LogicalOperator parentOperator)
        {
            var conditions = new List<ConditionModel>();

            if (!leftTree.IsGroup)
            {
                conditions.Add(leftTree);
                return conditions;
            }

            var prevOperator = leftTree.Conditions.Last().LogicalOperator;

            // (A AND B) AND RightTree -> Remove unnecessary parenthese
            // (A AND B) OR RightTree  -> Keep parenthese
            // (A OR B) AND RightTree  -> Keep parenthese
            // (A OR B) OR RightTree   -> Remove unnecessary parenthese
            if ((prevOperator == LogicalOperator.AND && parentOperator == LogicalOperator.AND)
                || (prevOperator == LogicalOperator.OR) && parentOperator == LogicalOperator.OR)
            {
                foreach (var child in leftTree.Conditions)
                {
                    conditions.Add(child);
                }

                return conditions;
            }
            else
            {
                conditions.Add(leftTree);
                return conditions;
            }
        }

        protected override void Visit(ConditionExpression exp)
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

            var param = _parameterFactory.FindByName(model.ParamName);
            model.ValueType = param.ValueType;

            _conditions.Push(model);
        }
    }
}