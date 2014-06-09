using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Expressions
{
    public abstract class ExpressionVisitor : IExpressionVisitor
    {
        protected virtual void Visit(Expression exp)
        {
            exp.Accept(this);
        }

        protected virtual void Visit(ComparisonExpression exp)
        {
        }

        protected virtual void Visit(ComparisonParamExpression exp)
        {
        }

        protected virtual void Visit(ComparisonValueExpression exp)
        {
        }

        protected virtual void Visit(LogicalBindaryExpression exp)
        {
            Visit(exp.Left);
            Visit(exp.Right);
        }

        #region Explicit Interface Members

        void IExpressionVisitor.Visit(Expression exp)
        {
            Visit(exp);
        }

        void IExpressionVisitor.Visit(ComparisonExpression exp)
        {
            Visit(exp);
        }

        void IExpressionVisitor.Visit(ComparisonParamExpression exp)
        {
            Visit(exp);
        }

        void IExpressionVisitor.Visit(ComparisonValueExpression exp)
        {
            Visit(exp);
        }

        void IExpressionVisitor.Visit(LogicalBindaryExpression exp)
        {
            Visit(exp);
        }

        #endregion
    }
}
