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
            return _conditions.Pop().Conditions;
        }

        protected override void Visit(LogicalBindaryExpression exp)
        {
            Visit(exp.Left);
            Visit(exp.Right);

            var rightTree = _conditions.Pop();
            var leftTree = _conditions.Pop();

            var group = new ConditionModel
            {
                IsGroup = true,
                LogicalOperator = exp.Operator
            };

            // A group is rendered as a block wrapped with a pair of parenthese.
            // Before building a conditon group, we need to check if we can trim the redundant parenthese for left and right trees.
            // For example, the parsed expression tree might be: (A AND B) OR C
            // In the UI, user will see (A AND B) as a group, so users will see two level conditions, which is not necessarily,
            // because Precedence(AND) > Precedence(OR).
            // 
            // We can flattern/simplify this expression to: A AND B OR C
            // So in the UI, user will see only one level conditions while the logic is still correct.

            // Try trim redundant parenthese for left tree
            foreach (var condition in TryTrimRedundantParentheseForLeftTree(leftTree, exp.Operator))
            {
                group.Conditions.Add(condition);
            }

            // Try trim redundant prenthese for right tree
            var first = true;

            foreach (var condition in TryTrimRedundantParentheseForRightTree(rightTree, exp.Operator))
            {
                if (first)
                {
                    condition.LogicalOperator = group.LogicalOperator;
                }
                group.Conditions.Add(condition);
                first = false;
            }

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

            // If left tree is a group, we need to check if this group can be flattern (all child conditions then be the children of the parent group).
            // We do this by checking the heading logical of the last child condition of the left tree.
            // For example (Expression in the parentheses represents the left tree):
            // (A AND B) AND RightTree -> Parenthese is redundant
            // (A AND B) OR RightTree  -> Parenthese is redundant
            // (A OR B) AND RightTree  -> Parenthese not redundant
            // (A OR B) OR RightTree   -> Parenthese is redundant
            if (!(prevOperator == LogicalOperator.Or && parentOperator == LogicalOperator.And))
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

        private List<ConditionModel> TryTrimRedundantParentheseForRightTree(ConditionModel rightTree, LogicalOperator parentOperator)
        {
            var conditions = new List<ConditionModel>();

            if (!rightTree.IsGroup)
            {
                conditions.Add(rightTree);
                return conditions;
            }

            var nextOperator = rightTree.Conditions.First().LogicalOperator;

            // If right tree is a group, we need to check if this group can be flattern (all child conditions then be the children of the parent group).
            // We do this by checking the heading logical of the first child condition of the right tree.
            // For example (Expression in the parentheses represents the right tree):
            // LeftTree AND (A AND B) -> Parenthese not redundant
            // LeftTree AND (A OR B)  -> Parenthese not redundant
            // LeftTree OR (A AND B)  -> Parenthese is redundant
            // LeftTree OR (A OR B)   -> Parenthese not redundant
            if (parentOperator == LogicalOperator.Or && nextOperator == LogicalOperator.And)
            {
                foreach (var child in rightTree.Conditions)
                {
                    conditions.Add(child);
                }

                return conditions;
            }
            else
            {
                conditions.Add(rightTree);
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