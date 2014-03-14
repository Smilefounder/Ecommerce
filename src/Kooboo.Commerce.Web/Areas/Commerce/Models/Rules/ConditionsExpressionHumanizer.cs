using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class ConditionsExpressionHumanizer : ExpressionVisitor
    {
        private StringBuilder _html;
        private IComparisonOperatorFactory _operatorFactory;
        private IConditionParameterFactory _paramFactory;
        private List<IConditionParameter> _parameters;

        public ConditionsExpressionHumanizer()
            : this(EngineContext.Current.Resolve<IComparisonOperatorFactory>(), EngineContext.Current.Resolve<IConditionParameterFactory>())
        {
        }

        public ConditionsExpressionHumanizer(
            IComparisonOperatorFactory comparisonOperatorFactory,
            IConditionParameterFactory paramFactory)
        {
            _operatorFactory = comparisonOperatorFactory;
            _paramFactory = paramFactory;
        }

        public string Humanize(string expression, Type contextModelType)
        {
            if (String.IsNullOrWhiteSpace(expression))
            {
                return String.Empty;
            }

            return Humanize(Expression.Parse(expression), contextModelType);
        }

        public string Humanize(Expression expression, Type contextModelType)
        {
            _html = new StringBuilder();
            _parameters = _paramFactory.GetConditionParameterInfos(contextModelType)
                                       .Select(x => x.Parameter)
                                       .ToList();

            Visit(expression);
            
            return _html.ToString();
        }

        protected override void Visit(LogicalBindaryExpression exp)
        {
            if (exp.Left is LogicalBindaryExpression)
            {
                _html.Append("<span class=\"parenthsis\">(</span>");
                Visit(exp.Left);
                _html.Append("<span class=\"parenthsis\">)</span>");
            }
            else
            {
                Visit(exp.Left);
            }

            _html.Append(" ")
                 .AppendFormat("<span class=\"keyword\">{0}</span>", exp.Operator)
                 .Append(" ");

            if (exp.Right is LogicalBindaryExpression)
            {
                _html.Append("<span class=\"parenthsis\">(</span>");
                Visit(exp.Right);
                _html.Append("<span class=\"parenthsis\">)</span>");
            }
            else
            {
                Visit(exp.Right);
            }
        }

        protected override void Visit(ConditionExpression exp)
        {
            _html.Append("<span class=\"flat-condition\">");

            Visit(exp.Param);

            _html.Append(" ")
                 .AppendFormat("<span class=\"operator\">{0}</span>", HumanizeComparisonOperator(exp.Operator))
                 .Append(" ");

            Visit(exp.Value);

            _html.Append("</span>");
        }

        protected override void Visit(ConditionParamExpression exp)
        {
            var paramDisplayName = exp.ParamName;
            var param = _parameters.FirstOrDefault(x => x.Name.Equals(exp.ParamName, StringComparison.OrdinalIgnoreCase));
            if (param != null)
            {
                paramDisplayName = param.DisplayName;
            }

            _html.AppendFormat("<span class=\"param\">{0}</span>", paramDisplayName);
        }

        protected override void Visit(ConditionValueExpression exp)
        {
            _html.Append("<span class=\"value");

            var isString = exp.ValueType == typeof(String);

            if (isString)
            {
                _html.Append(" string");
            }
            else
            {
                _html.Append(" number");
            }

            _html.Append("\">");

            if (isString)
            {
                _html.AppendFormat("\"{0}\"", exp.Value);
            }
            else
            {
                _html.Append(exp.Value);
            }

            _html.Append("</span>");
        }

        private string HumanizeComparisonOperator(string @operator)
        {
            var op = _operatorFactory.FindByName(@operator);
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
    }
}