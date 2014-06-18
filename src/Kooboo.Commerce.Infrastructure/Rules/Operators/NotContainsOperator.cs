using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    public class NotContainsOperator : IComparisonOperator
    {
        public string Name
        {
            get
            {
                return "not contains";
            }
        }

        public string Alias
        {
            get
            {
                return null;
            }
        }

        public bool Apply(ConditionParameter param, object paramValue, object inputValue)
        {
            return !ComparisonOperators.Contains.Apply(param, paramValue, inputValue);
        }
    }
}
