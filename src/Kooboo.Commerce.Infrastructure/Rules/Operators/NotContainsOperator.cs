using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    [Dependency(typeof(IComparisonOperator), ComponentLifeStyle.Singleton, Key = "not_contains")]
    public class NotContainsOperator : IComparisonOperator
    {
        public string Name
        {
            get
            {
                return "not_contains";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Not Contains";
            }
        }

        public bool Apply(IConditionParameter param, object paramValue, object inputValue)
        {
            return !ComparisonOperators.Contains.Apply(param, paramValue, inputValue);
        }
    }
}
