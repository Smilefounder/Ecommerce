using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    [Dependency(typeof(IComparisonOperator), ComponentLifeStyle.Singleton, Key = "contains")]
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
            Require.NotNull(param, "param");
            Require.NotNull(paramValue, "paramValue");
            Require.NotNull(inputValue, "inputValue");

            return paramValue.ToString().Contains(inputValue.ToString());
        }
    }
}
