using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    public class GreaterThanOperator : IComparisonOperator
    {
        public string Name
        {
            get { return "greater than"; }
        }

        public string Alias
        {
            get
            {
                return ">";
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
