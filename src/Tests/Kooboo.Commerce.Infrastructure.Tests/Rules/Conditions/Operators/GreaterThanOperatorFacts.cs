﻿using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kooboo.Commerce.Infrastructure.Tests.Rules.Operators
{
    public class GreaterThanOperatorFacts
    {
        [Fact]
        public void can_compare_integers()
        {
            Assert.True(new ConditionEvaluator().Evaluate("Int32Param > 5", new ContextModel
            {
                Int32Param = 6
            }));
            Assert.True(new ConditionEvaluator().Evaluate("Int32Param > 50", new ContextModel
            {
                Int32Param = 100
            }));
            Assert.False(new ConditionEvaluator().Evaluate("Int32Param > 5", new ContextModel
            {
                Int32Param = 5
            }));
            Assert.False(new ConditionEvaluator().Evaluate("Int32Param > 10", new ContextModel
            {
                Int32Param = 5
            }));
        }

        [Fact]
        public void can_compare_decimals()
        {
            Assert.True(new ConditionEvaluator().Evaluate("DecimalParam > 5.2", new ContextModel
            {
                DecimalParam = 5.3m
            }));
            Assert.False(new ConditionEvaluator().Evaluate("DecimalParam > 5.2", new ContextModel
            {
                DecimalParam = 5.2m
            }));
        }

        [Fact]
        public void can_compare_nullable_values()
        {
            Assert.True(new ConditionEvaluator().Evaluate("NullableInt32Param > 5", new ContextModel
            {
                NullableInt32Param = 6
            }));
            Assert.False(new ConditionEvaluator().Evaluate("NullableInt32Param > 5", new ContextModel
            {
                NullableInt32Param = 4
            }));
            Assert.False(new ConditionEvaluator().Evaluate("NullableInt32Param > 5", new ContextModel
            {
                NullableInt32Param = null
            }));
        }
    }
}
