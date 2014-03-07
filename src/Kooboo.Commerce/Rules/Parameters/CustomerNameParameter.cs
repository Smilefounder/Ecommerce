using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    [Dependency(typeof(IConditionParameter), Key = "CustomerName")]
    public class CustomerNameParameter : IConditionParameter
    {
        public string Name
        {
            get { return "CustomerName"; }
        }

        public string DisplayName
        {
            get { return "Customer Name"; }
        }

        public Type ModelType
        {
            get { return typeof(Customer); }
        }

        public ParameterValueType ValueType
        {
            get
            {
                return ParameterValueType.String;
            }
        }

        public IEnumerable<IComparisonOperator> SupportedOperators
        {
            get
            {
                return new[] {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals,
                    ComparisonOperators.Contains,
                    ComparisonOperators.NotEquals
                };
            }
        }

        public object GetValue(object model)
        {
            var customer = (Customer)model;
            return customer.FullName;
        }
    }
}
