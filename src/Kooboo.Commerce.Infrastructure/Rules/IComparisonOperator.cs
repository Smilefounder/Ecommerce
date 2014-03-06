using Kooboo.Commerce.Rules.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IComparisonOperator
    {
        string Name { get; }

        string DisplayName { get; }

        bool Apply(IParameter param, object paramValue, object inputValue);
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
    }
}
