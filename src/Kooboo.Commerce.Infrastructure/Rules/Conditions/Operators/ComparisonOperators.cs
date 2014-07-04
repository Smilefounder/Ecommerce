using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Operators
{
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

        static readonly ComparisonOperatorCollection _operators;

        public static ComparisonOperatorCollection Operators
        {
            get
            {
                return _operators;
            }
        }

        static ComparisonOperators()
        {
            _operators = new ComparisonOperatorCollection
            {
                ComparisonOperators.Equals,
                ComparisonOperators.NotEquals,
                ComparisonOperators.GreaterThan,
                ComparisonOperators.GreaterThanOrEqual,
                ComparisonOperators.LessThan,
                ComparisonOperators.LessThanOrEqual,
                ComparisonOperators.Contains,
                ComparisonOperators.NotContains
            };
        }
    }
}
