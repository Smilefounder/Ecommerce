using Kooboo.Commerce.Rules.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// 定义用于条件表达式中的比较运算符接口。
    /// </summary>
    public interface IComparisonOperator
    {
        string Name { get; }

        string DisplayName { get; }

        /// <summary>
        /// 在参数上应用运算符。
        /// </summary>
        /// <param name="param">参数。</param>
        /// <param name="paramValue">参数值。</param>
        /// <param name="inputValue">用于比较的值。</param>
        /// <returns>比较结果，true表示成功，false表示失败。</returns>
        bool Apply(ConditionParameter param, object paramValue, object inputValue);
    }

    public static class ComparisonOperators
    {
        public static new readonly IComparisonOperator Equals = new EqualsOperator();

        public static readonly IComparisonOperator NotEquals = new NotEqualsOperator();

        public static readonly IComparisonOperator GreaterThan = new GreaterThanOperator();

        public static readonly IComparisonOperator GreaterThanOrEqual = new GreaterThanOrEqualOperator();

        public static readonly IComparisonOperator LessThan = new LessThanOperator();

        public static readonly IComparisonOperator LessThanOrEqual = new LessThanOrEqualOperator();

        public static readonly IComparisonOperator Contains = new ContainsOperator();

        public static readonly IComparisonOperator NotContains = new NotContainsOperator();

        public static string TryGetOperatorShortcut(string operatorName)
        {
            if (operatorName == Equals.Name)
            {
                return "==";
            }
            if (operatorName == NotEquals.Name)
            {
                return "!=";
            }
            if (operatorName == GreaterThan.Name)
            {
                return ">";
            }
            if (operatorName == GreaterThanOrEqual.Name)
            {
                return ">=";
            }
            if (operatorName == LessThan.Name)
            {
                return "<";
            }
            if (operatorName == LessThanOrEqual.Name)
            {
                return "<=";
            }

            return operatorName;
        }

        public static IComparisonOperator GetOperatorFromShortcut(string shortcut)
        {
            switch (shortcut)
            {
                case "==":
                    return ComparisonOperators.Equals;
                case "!=":
                    return ComparisonOperators.NotEquals;
                case ">":
                    return ComparisonOperators.GreaterThan;
                case ">=":
                    return ComparisonOperators.GreaterThanOrEqual;
                case "<":
                    return ComparisonOperators.LessThan;
                case "<=":
                    return ComparisonOperators.LessThanOrEqual;
                default:
                    return null;
            }
        }
    }
}
