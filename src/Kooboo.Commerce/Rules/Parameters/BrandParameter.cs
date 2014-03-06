using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    [Dependency(typeof(IConditionParameter), Key = "Brand")]
    public class BrandParameter : IConditionParameter
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
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals
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
