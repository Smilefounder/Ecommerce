using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Expressions.Formatting
{
    public class HtmlExpressionFormatter : ExpressionFormatter
    {
        protected override void WriteOpenParenthsis()
        {
            BeginSpan("parenthsis");
            base.WriteOpenParenthsis();
            EndSpan();
        }

        protected override void WriteCloseParenthsis()
        {
            BeginSpan("parenthsis");
            base.WriteCloseParenthsis();
            EndSpan();
        }

        protected override void WriteKeyword(string value)
        {
            BeginSpan("keyword");
            base.WriteKeyword(value);
            EndSpan();
        }

        protected override void WriteLeafCondition(ComparisonExpression exp)
        {
            BeginSpan("flat-condition");
            base.WriteLeafCondition(exp);
            EndSpan();
        }

        protected override void WriteParamName(string value)
        {
            BeginSpan("param");
            base.WriteParamName(value);
            EndSpan();
        }

        protected override void WriteParamValue(string value, Type valueType)
        {
            BeginSpan("value");
            base.WriteParamValue(value, valueType);
            EndSpan();
        }

        protected override void WriteOperator(string value)
        {
            BeginSpan("operator");
            base.WriteOperator(value);
            EndSpan();
        }

        private void BeginSpan(string className)
        {
            Write(String.Format("<span class=\"{0}\">", className));
        }

        private void EndSpan()
        {
            Write("</span>");
        }
    }
}
