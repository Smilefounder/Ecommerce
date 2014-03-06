using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    public class GreaterThanOrEqualOperator : IComparisonOperator
    {
        public string Name
        {
            get
            {
                return "greater_than_or_equal";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Greater Than Or Equal";
            }
        }

        public bool Apply(IParameter param, object paramValue, object inputValue)
        {
            return ComparisonOperators.GreaterThan.Apply(param, paramValue, inputValue)
                || ComparisonOperators.Equals.Apply(param, paramValue, inputValue);
        }
    }
}
