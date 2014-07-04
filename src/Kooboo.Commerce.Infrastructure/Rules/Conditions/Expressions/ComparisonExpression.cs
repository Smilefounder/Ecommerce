using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Expressions
{
    public class ComparisonExpression : Expression
    {
        public ComparisonParamExpression Param { get; private set; }

        public ComparisonValueExpression Value { get; private set; }

        public string Operator { get; private set; }

        public ComparisonExpression(ComparisonParamExpression param, ComparisonValueExpression value, string @operator)
        {
            Require.NotNull(param, "param");
            Require.NotNull(value, "value");
            Require.NotNullOrEmpty(@operator, "operator");

            Param = param;
            Value = value;
            Operator = @operator;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            Require.NotNull(visitor, "visitor");
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Param + " " + Operator + " " + Value;
        }
    }
}
