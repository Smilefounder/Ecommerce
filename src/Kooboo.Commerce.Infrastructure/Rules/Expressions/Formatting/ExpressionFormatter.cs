using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Expressions.Formatting
{
    public abstract class ExpressionFormatter : ExpressionVisitor
    {
        private StringBuilder _html;

        protected ComparisonOperatorManager OperatorManager { get; private set; }

        protected ExpressionFormatter()
            : this(ComparisonOperatorManager.Instance)
        {
        }

        protected ExpressionFormatter(ComparisonOperatorManager operatorManager)
        {
            OperatorManager = operatorManager;
        }

        public string Format(string expression, Type dataContextType)
        {
            if (String.IsNullOrWhiteSpace(expression))
            {
                return String.Empty;
            }

            return Format(Expression.Parse(expression, OperatorManager.Operators.Select(o => o.Name).ToList()), dataContextType);
        }

        public string Format(Expression expression, Type dataContextType)
        {
            _html = new StringBuilder();
            Visit(expression);

            return _html.ToString();
        }

        protected virtual void Write(string value)
        {
            _html.Append(value);
        }

        protected virtual void WriteSpace()
        {
            _html.Append(" ");
        }

        protected virtual void WriteOpenParenthsis()
        {
            Write("(");
        }

        protected virtual void WriteCloseParenthsis()
        {
            Write(")");
        }

        protected virtual void WriteKeyword(string value)
        {
            Write(value);
        }

        protected virtual void WriteOperator(string value)
        {
            Write(value);
        }

        protected virtual void WriteLeafCondition(ComparisonExpression exp)
        {
            Visit(exp.Param);

            WriteSpace();
            WriteKeyword(GetFriendlyOperator(exp.Operator));
            WriteSpace();

            Visit(exp.Value);
        }

        protected virtual void WriteParamName(string value)
        {
            Write(value);
        }

        protected virtual void WriteParamValue(string value, Type valueType)
        {
            var isString = valueType == typeof(String);
            if (isString)
            {
                Write(String.Format("\"{0}\"", value));
            }
            else
            {
                Write(value);
            }
        }

        #region Visitor Methods

        protected sealed override void Visit(Expression exp)
        {
            base.Visit(exp);
        }

        protected sealed override void Visit(LogicalBindaryExpression exp)
        {
            if (exp.Left is LogicalBindaryExpression)
            {
                WriteOpenParenthsis();
                Visit(exp.Left);
                WriteCloseParenthsis();
            }
            else
            {
                Visit(exp.Left);
            }

            WriteSpace();
            WriteOperator(exp.Operator.ToString());
            WriteSpace();

            if (exp.Right is LogicalBindaryExpression)
            {
                WriteOpenParenthsis();
                Visit(exp.Right);
                WriteCloseParenthsis();
            }
            else
            {
                Visit(exp.Right);
            }
        }

        protected sealed override void Visit(ComparisonExpression exp)
        {
            WriteLeafCondition(exp);
        }

        protected sealed override void Visit(ComparisonParamExpression exp)
        {
            WriteParamName(exp.ParamName);
        }

        protected sealed override void Visit(ComparisonValueExpression exp)
        {
            WriteParamValue(exp.Value, exp.ValueType);
        }

        private string GetFriendlyOperator(string @operator)
        {
            var op = OperatorManager.Find(@operator);
            if (op == null)
            {
                op = ComparisonOperators.GetOperatorFromShortcut(@operator);
            }

            if (op != null)
            {
                return op.Name.Replace('_', ' ');
            }

            return @operator;
        }

        #endregion
    }
}
