using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    [Dependency(typeof(IComparisonOperator), ComponentLifeStyle.Singleton, Key = "greater_than")]
    public class GreaterThanOperator : IComparisonOperator
    {
        public string Name
        {
            get { return "greater_than"; }
        }

        public string DisplayName
        {
            get
            {
                return "Greater Than";
            }
        }

        public bool Apply(ConditionParameter param, object paramValue, object inputValue)
        {
            Require.NotNull(param, "param");
            Require.NotNull(paramValue, "paramValue");
            Require.That(paramValue is IComparable, "paramValue", "Require comparable parameter value.");
            Require.NotNull(inputValue, "inputValue");
            Require.That(inputValue is IComparable, "inputValue", "Require comparable input value.");

            return ((IComparable)paramValue).CompareTo((IComparable)inputValue) > 0;
        }
    }
}
