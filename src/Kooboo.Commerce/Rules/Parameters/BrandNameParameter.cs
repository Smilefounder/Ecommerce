using Kooboo.Commerce.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    public class BrandNameParameter : IParameter
    {
        public string Name
        {
            get
            {
                return "BrandName";
            }
        }

        public Type ModelType
        {
            get
            {
                return typeof(Brand);
            }
        }

        public Type ValueType
        {
            get
            {
                return typeof(string);
            }
        }

        public IEnumerable<IComparisonOperator> SupportedOperators
        {
            get
            {
                return new IComparisonOperator[] {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals,
                    ComparisonOperators.Contains,
                    ComparisonOperators.NotContains
                };
            }
        }

        public object GetValue(object context)
        {
            var brand = (Brand)context;
            return brand.Name;
        }
    }
}
