using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IComparisonOperator
    {
        bool Apply(IParameter param, object paramValue, object inputValue);
    }

    public static class ComparisonOperators
    {
        public static readonly IComparisonOperator Equal = new EqualOperator();

        public static readonly IComparisonOperator NotEqual = new ReverseOperator(Equal);

        public static readonly IComparisonOperator Contain = new ContainOperator();

        public static readonly IComparisonOperator NotContain = new ReverseOperator(Contain);
    }

    public class EqualOperator : IComparisonOperator
    {
        public bool Apply(IParameter param, object paramValue, object inputValue)
        {
            return paramValue.Equals(inputValue);
        }
    }

    public class ReverseOperator : IComparisonOperator
    {
        private IComparisonOperator _innerOperator;

        public ReverseOperator(IComparisonOperator innerOperator)
        {
            _innerOperator = innerOperator;
        }

        public bool Apply(IParameter param, object paramValue, object inputValue)
        {
            return !_innerOperator.Apply(param, paramValue, inputValue);
        }
    }

    public class ContainOperator : IComparisonOperator
    {
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
