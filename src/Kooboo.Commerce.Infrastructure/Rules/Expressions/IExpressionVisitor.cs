using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Expressions
{
    public interface IExpressionVisitor
    {
        void Visit(Expression exp);

        void Visit(ComparisonExpression exp);

        void Visit(ComparisonParamExpression exp);

        void Visit(ComparisonValueExpression exp);

        void Visit(LogicalBindaryExpression exp);
    }
}
