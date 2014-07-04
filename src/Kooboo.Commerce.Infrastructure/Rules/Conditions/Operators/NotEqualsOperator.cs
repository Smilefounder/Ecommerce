using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Operators
{
    public class NotEqualsOperator : IComparisonOperator
    {
        public string Name
        {
            get
            {
                return "not equals";
            }
        }

        public string Alias
        {
            get
            {
                return "!=";
            }
        }

        public bool Apply(RuleParameter param, object paramValue, object inputValue)
        {
            return !ComparisonOperators.Equals.Apply(param, paramValue, inputValue);
        }
    }
}
