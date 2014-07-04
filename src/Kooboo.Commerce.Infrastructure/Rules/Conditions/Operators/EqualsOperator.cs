using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Operators
{
    public class EqualsOperator : IComparisonOperator
    {
        public string Name
        {
            get { return "equals"; }
        }

        public string Alias
        {
            get
            {
                return "==";
            }
        }

        public bool Apply(ConditionParameter param, object paramValue, object inputValue)
        {
            if (paramValue is String && inputValue is String)
            {
                return (paramValue as String).Equals((inputValue as String), StringComparison.OrdinalIgnoreCase);
            }

            return paramValue.Equals(inputValue);
        }
    }
}
