﻿using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    [Dependency(typeof(IConditionParameter), Key = "BrandName")]
    public class BrandNameParameter : IConditionParameter
    {
        public string Name
        {
            get { return "BrandName"; }
        }

        public string DisplayName
        {
            get { return "Brand Name"; }
        }

        public Type ModelType
        {
            get
            {
                return typeof(Brand);
            }
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
