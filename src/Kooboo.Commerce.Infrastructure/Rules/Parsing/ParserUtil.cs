using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parsing
{
    static class ParserUtil
    {
        public static IComparisonOperator ParseBuiltinComparisonOperator(string @operator)
        {
            switch (@operator)
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
