using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Expressions
{
    public class ComparisonValueExpression : Expression
    {
        public string Value { get; private set; }

        public Type ValueType { get; private set; }

        public ComparisonValueExpression(string value, Type valueType)
        {
            Value = value;
            ValueType = valueType;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            Require.NotNull(visitor, "visitor");
            visitor.Visit(this);
        }

        public override string ToString()
        {
            if (ValueType.IsNumericType())
            {
                return Value;
            }

            return "\"" + Value + "\"";
        }
    }
}
