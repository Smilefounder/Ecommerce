using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Expressions
{
    public class ConditionValueExpression : Expression
    {
        public string Value { get; private set; }

        public Type ValueType { get; private set; }

        public ConditionValueExpression(string value, Type valueType)
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
            if (ValueType.IsNumber())
            {
                return Value;
            }

            return "\"" + Value + "\"";
        }
    }
}
