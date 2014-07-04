using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Operators
{
    public class ContainsOperator : IComparisonOperator
    {
        public string Name
        {
            get { return "contains"; }
        }

        public string Alias
        {
            get
            {
                return null;
            }
        }

        public bool Apply(RuleParameter param, object paramValue, object inputValue)
        {
            Require.NotNull(param, "param");
            Require.NotNull(paramValue, "paramValue");
            Require.NotNull(inputValue, "inputValue");

            return paramValue.ToString().Contains(inputValue.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
