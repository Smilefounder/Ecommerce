using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    [Dependency(typeof(IComparisonOperator), ComponentLifeStyle.Singleton, Key = "greater_than_or_equal")]
    public class GreaterThanOrEqualOperator : IComparisonOperator
    {
        public string Name
        {
            get
            {
                return "greater than or equal";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Greater Than Or Equal";
            }
        }

        public bool Apply(ConditionParameter param, object paramValue, object inputValue)
        {
            return ComparisonOperators.GreaterThan.Apply(param, paramValue, inputValue)
                || ComparisonOperators.Equals.Apply(param, paramValue, inputValue);
        }
    }
}
