using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    public class ContainsOperator : IComparisonOperator
    {
        public string Name
        {
            get { return "contains"; }
        }

        public string DisplayName
        {
            get { return "Contains"; }
        }

        public bool Apply(IParameter param, object paramValue, object inputValue)
        {
            if (paramValue == null || inputValue == null)
            {
                return false;
            }

            return paramValue.ToString().Contains(inputValue.ToString());
        }
    }
}
