using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Expressions
{
    public class LogicalBindaryExpression : Expression
    {
        public Expression Left { get; private set; }

        public Expression Right { get; private set; }

        public LogicalOperator Operator { get; private set; }

        public LogicalBindaryExpression(Expression left, Expression right, LogicalOperator @operator)
        {
            Require.NotNull(left, "left");
            Require.NotNull(right, "right");

            Left = left;
            Right = right;
            Operator = @operator;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            Require.NotNull(visitor, "visitor");
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var leftTree = Left.ToString();
            var rightTree = Right.ToString();

            if ((Left is LogicalBindaryExpression) && ((LogicalBindaryExpression)Left).Operator == LogicalOperator.OR)
            {
                leftTree = "(" + leftTree + ")";
            }
            if ((Right is LogicalBindaryExpression) && ((LogicalBindaryExpression)Right).Operator == LogicalOperator.OR)
            {
                rightTree = "(" + rightTree + ")";
            }

            return leftTree + " " + Operator + " " + rightTree;
        }
    }
}
