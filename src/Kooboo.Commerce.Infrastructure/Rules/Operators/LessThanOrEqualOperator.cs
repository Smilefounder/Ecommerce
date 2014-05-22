using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    [Dependency(typeof(IComparisonOperator), ComponentLifeStyle.Singleton, Key = "less_than_or_equal")]
    public class LessThanOrEqualOperator : IComparisonOperator
    {
        public string Name
        {
            get
            {
                return "less_than_or_equal";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Less Than Or Equal";
            }
        }

        public bool Apply(ConditionParameter param, object paramValue, object inputValue)
        {
            return !ComparisonOperators.GreaterThan.Apply(param, paramValue, inputValue);
        }
    }
}
