using Kooboo.Commerce.Rules.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Expressions
{
    /// <summary>
    /// Represents a node in the abstract syntax tree.
    /// </summary>
    public abstract class Expression
    {
        public abstract void Accept(IExpressionVisitor visitor);

        public static Expression Parse(string source)
        {
            return new Parser().Parse(source);
        }

        public static ConditionParamExpression ConditionParam(string paramName)
        {
            return new ConditionParamExpression(paramName);
        }

        public static ConditionValueExpression ConditionValue(string value, string dataSourceId = null)
        {
            return new ConditionValueExpression(value, dataSourceId);
        }

        public static ConditionExpression Condition(ConditionParamExpression param, ConditionValueExpression value, string @operator)
        {
            return new ConditionExpression(param, value, @operator);
        }

        public static LogicalBindaryExpression Binary(Expression left, Expression right, LogicalOperator @operator)
        {
            return new LogicalBindaryExpression(left, right, @operator);
        }
    }
}
