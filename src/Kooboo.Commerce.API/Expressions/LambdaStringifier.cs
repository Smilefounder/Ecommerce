using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.API.Expressions
{
    public class LambdaStringifier : ExpressionVisitor
    {
        private StringBuilder _result;

        public string Stringify(Expression lambda)
        {
            if (lambda.NodeType != ExpressionType.Lambda)
                throw new ArgumentException("Requires labmda expression.", "lambda");

            _result = new StringBuilder();
            Visit(lambda);
            return _result.ToString();
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);

            _result.Append(" ");

            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    _result.Append("+");
                    break;
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    _result.Append("AND");
                    break;
                case ExpressionType.Divide:
                    _result.Append("/");
                    break;
                case ExpressionType.Equal:
                    _result.Append("==");
                    break;
                case ExpressionType.GreaterThan:
                    _result.Append(">");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _result.Append(">=");
                    break;
                case ExpressionType.LessThan:
                    _result.Append("<");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _result.Append("<=");
                    break;
                case ExpressionType.Multiply:
                    _result.Append("*");
                    break;
                case ExpressionType.NotEqual:
                    _result.Append("!=");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _result.Append("OR");
                    break;
                case ExpressionType.Subtract:
                    _result.Append("-");
                    break;
                default:
                    throw new NotSupportedException();
            }

            _result.Append(" ");

            Visit(node.Right);

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _result.Append(node.Member.Name);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value == null)
            {
                _result.Append("null");
            }
            else if (node.GetType() == typeof(string))
            {
                _result.AppendFormat("\"{0}\"", node.Value);
            }
            else
            {
                _result.Append(node.Value);
            }

            return node;
        }
    }
}
