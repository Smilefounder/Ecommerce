using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Expressions
{
    public class ComparisonParamExpression : Expression
    {
        public string ParamName { get; private set; }

        public ComparisonParamExpression(string paramName)
        {
            Require.NotNullOrEmpty(paramName, "paramName");
            ParamName = paramName;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            Require.NotNull(visitor, "visitor");
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return ParamName;
        }
    }
}
