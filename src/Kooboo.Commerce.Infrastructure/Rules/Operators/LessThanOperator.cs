using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    [Dependency(typeof(IComparisonOperator), ComponentLifeStyle.Singleton, Key = "less_than")]
    public class LessThanOperator : IComparisonOperator
    {
        public string Name
        {
            get
            {
                return "less than";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Less Than";
            }
        }

        public bool Apply(ConditionParameter param, object paramValue, object inputValue)
        {
            return !ComparisonOperators.GreaterThanOrEqual.Apply(param, paramValue, inputValue);
        }
    }
}
