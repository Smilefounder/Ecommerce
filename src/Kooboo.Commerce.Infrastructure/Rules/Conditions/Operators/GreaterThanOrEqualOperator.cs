﻿using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Operators
{
    public class GreaterThanOrEqualOperator : IComparisonOperator
    {
        public string Name
        {
            get
            {
                return "greater than or equal";
            }
        }

        public string Alias
        {
            get
            {
                return ">=";
            }
        }

        public bool Apply(RuleParameter param, object paramValue, object inputValue)
        {
            return ComparisonOperators.GreaterThan.Apply(param, paramValue, inputValue)
                || ComparisonOperators.Equals.Apply(param, paramValue, inputValue);
        }
    }
}
