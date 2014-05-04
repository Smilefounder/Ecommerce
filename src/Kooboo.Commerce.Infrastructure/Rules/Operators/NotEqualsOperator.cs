using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    [Dependency(typeof(IComparisonOperator), ComponentLifeStyle.Singleton, Key = "not_equals")]
    public class NotEqualsOperator : IComparisonOperator
    {
        public string Name
        {
            get
            {
                return "not_equals";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Not Equals";
            }
        }

        public bool Apply(IConditionParameter param, object paramValue, object inputValue)
        {
            return !ComparisonOperators.Equals.Apply(param, paramValue, inputValue);
        }
    }
}
