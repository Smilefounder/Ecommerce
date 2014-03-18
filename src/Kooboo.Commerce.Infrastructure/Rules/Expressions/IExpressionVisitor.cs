using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Expressions
{
    public interface IExpressionVisitor
    {
        void Visit(Expression exp);

        void Visit(ConditionExpression exp);

        void Visit(ConditionParamExpression exp);

        void Visit(ConditionValueExpression exp);

        void Visit(LogicalBindaryExpression exp);
    }
}
