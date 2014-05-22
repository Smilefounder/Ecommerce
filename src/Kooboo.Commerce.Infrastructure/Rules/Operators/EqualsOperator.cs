using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    [Dependency(typeof(IComparisonOperator), ComponentLifeStyle.Singleton, Key = "equals")]
    public class EqualsOperator : IComparisonOperator
    {
        public string Name
        {
            get { return "equals"; }
        }

        public string DisplayName
        {
            get
            {
                return "Equals";
            }
        }

        public bool Apply(ConditionParameter param, object paramValue, object inputValue)
        {
            return paramValue.Equals(inputValue);
        }
    }
}
