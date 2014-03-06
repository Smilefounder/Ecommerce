using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    public class LessThanOperator : IComparisonOperator
    {
        public string Name
        {
            get
            {
                return "less_than";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Less Than";
            }
        }

        public bool Apply(IParameter param, object paramValue, object inputValue)
        {
            return !ComparisonOperators.GreaterThanOrEqual.Apply(param, paramValue, inputValue);
        }
    }
}
