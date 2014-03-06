using Kooboo.Commerce.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    public class BrandParameter : IParameter
    {
        public string Name
        {
            get
            {
                return "Brand";
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
                return typeof(Int32);
            }
        }

        public IEnumerable<IComparisonOperator> SupportedOperators
        {
            get
            {
                return new IComparisonOperator[] {
                    ComparisonOperators.Equal,
                    ComparisonOperators.NotEqual
                };
            }
        }

        public object GetValue(object context)
        {
            var brand = (Brand)context;
            return brand.Id;
        }
    }
}
