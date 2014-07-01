using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kooboo.Commerce.Infrastructure.Tests.Rules.Operators
{
    public class EqualsOperatorFacts
    {
        [Fact]
        public void can_compare_strings()
        {
            Assert.True(new ConditionEvaluator().Evaluate("StringParam == \"StrValue\"", new ContextModel
            {
                StringParam = "StrValue"
            }));
            Assert.False(new ConditionEvaluator().Evaluate("StringParam == \"StrValue\"", new ContextModel
            {
                StringParam = "The Value"
            }));
        }

        [Fact]
        public void can_compare_numbers()
        {
            // Integers
            Assert.True(new ConditionEvaluator().Evaluate("Int32Param == 5", new ContextModel
            {
                Int32Param = 5
            }));
            Assert.False(new ConditionEvaluator().Evaluate("Int32Param == 5", new ContextModel
            {
                Int32Param = 8
            }));

            // Decimals
            Assert.True(new ConditionEvaluator().Evaluate("DecimalParam == 52", new ContextModel
            {
                DecimalParam = 52
            }));
            Assert.True(new ConditionEvaluator().Evaluate("DecimalParam == 52.23", new ContextModel
            {
                DecimalParam = 52.23m
            }));

            Assert.False(new ConditionEvaluator().Evaluate("DecimalParam == 52", new ContextModel
            {
                DecimalParam = 53
            }));
            Assert.False(new ConditionEvaluator().Evaluate("DecimalParam == 52", new ContextModel
            {
                DecimalParam = 52.23m
            }));
            Assert.False(new ConditionEvaluator().Evaluate("DecimalParam == 52.51", new ContextModel
            {
                DecimalParam = 52.5m
            }));

            // Floats
            Assert.True(new ConditionEvaluator().Evaluate("FloatParam == 5.2", new ContextModel
            {
                FloatParam = 5.2f
            }));

            // Doubles
            Assert.True(new ConditionEvaluator().Evaluate("DoubleParam == 5.345", new ContextModel
            {
                DoubleParam = 5.345d
            }));
            Assert.False(new ConditionEvaluator().Evaluate("DoubleParam == 5.345", new ContextModel
            {
                DoubleParam = 5.3451d
            }));
        }

        [Fact]
        public void can_compare_enum()
        {
            Assert.True(new ConditionEvaluator().Evaluate("EnumParam == \"Value2\"", new ContextModel
            {
                EnumParam = SampleEnum.Value2
            }));
            Assert.False(new ConditionEvaluator().Evaluate("EnumParam == \"Value2\"", new ContextModel
            {
                EnumParam = SampleEnum.Value3
            }));
        }

        [Fact]
        public void can_compare_nullable_values()
        {
            Assert.True(new ConditionEvaluator().Evaluate("NullableDecimalParam == 5", new ContextModel
            {
                NullableDecimalParam = 5
            }));
            Assert.False(new ConditionEvaluator().Evaluate("NullableDecimalParam == 5", new ContextModel
            {
                NullableDecimalParam = 6
            }));
            Assert.False(new ConditionEvaluator().Evaluate("NullableDecimalParam == 5", new ContextModel
            {
                NullableDecimalParam = null
            }));

            Assert.True(new ConditionEvaluator().Evaluate("NullableEnumParam == \"Value2\"", new ContextModel
            {
                NullableEnumParam = SampleEnum.Value2
            }));
            Assert.False(new ConditionEvaluator().Evaluate("NullableEnumParam == \"Value2\"", new ContextModel
            {
                NullableEnumParam = SampleEnum.Value1
            }));
            Assert.False(new ConditionEvaluator().Evaluate("NullableEnumParam == \"Value2\"", new ContextModel
            {
                NullableEnumParam = null
            }));
        }

        [Fact]
        public void should_ignore_case()
        {
            Assert.True(new ConditionEvaluator().Evaluate("StringParam == \"kooboo\"", new ContextModel
            {
                StringParam = "Kooboo"
            }));
        }
    }
}
