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
            Assert.True(new RuleEngine().CheckCondition("StringParam == \"StrValue\"", new ContextModel
            {
                StringParam = "StrValue"
            }));
            Assert.False(new RuleEngine().CheckCondition("StringParam == \"StrValue\"", new ContextModel
            {
                StringParam = "The Value"
            }));
        }

        [Fact]
        public void can_compare_numbers()
        {
            // Integers
            Assert.True(new RuleEngine().CheckCondition("Int32Param == 5", new ContextModel
            {
                Int32Param = 5
            }));
            Assert.False(new RuleEngine().CheckCondition("Int32Param == 5", new ContextModel
            {
                Int32Param = 8
            }));

            // Decimals
            Assert.True(new RuleEngine().CheckCondition("DecimalParam == 52", new ContextModel
            {
                DecimalParam = 52
            }));
            Assert.True(new RuleEngine().CheckCondition("DecimalParam == 52.23", new ContextModel
            {
                DecimalParam = 52.23m
            }));

            Assert.False(new RuleEngine().CheckCondition("DecimalParam == 52", new ContextModel
            {
                DecimalParam = 53
            }));
            Assert.False(new RuleEngine().CheckCondition("DecimalParam == 52", new ContextModel
            {
                DecimalParam = 52.23m
            }));
            Assert.False(new RuleEngine().CheckCondition("DecimalParam == 52.51", new ContextModel
            {
                DecimalParam = 52.5m
            }));

            // Floats
            Assert.True(new RuleEngine().CheckCondition("FloatParam == 5.2", new ContextModel
            {
                FloatParam = 5.2f
            }));

            // Doubles
            Assert.True(new RuleEngine().CheckCondition("DoubleParam == 5.345", new ContextModel
            {
                DoubleParam = 5.345d
            }));
            Assert.False(new RuleEngine().CheckCondition("DoubleParam == 5.345", new ContextModel
            {
                DoubleParam = 5.3451d
            }));
        }

        [Fact]
        public void can_compare_enum()
        {
            Assert.True(new RuleEngine().CheckCondition("EnumParam == \"Value2\"", new ContextModel
            {
                EnumParam = SampleEnum.Value2
            }));
            Assert.False(new RuleEngine().CheckCondition("EnumParam == \"Value2\"", new ContextModel
            {
                EnumParam = SampleEnum.Value3
            }));
        }

        [Fact]
        public void can_compare_nullable_values()
        {
            Assert.True(new RuleEngine().CheckCondition("NullableDecimalParam == 5", new ContextModel
            {
                NullableDecimalParam = 5
            }));
            Assert.False(new RuleEngine().CheckCondition("NullableDecimalParam == 5", new ContextModel
            {
                NullableDecimalParam = 6
            }));
            Assert.False(new RuleEngine().CheckCondition("NullableDecimalParam == 5", new ContextModel
            {
                NullableDecimalParam = null
            }));

            Assert.True(new RuleEngine().CheckCondition("NullableEnumParam == \"Value2\"", new ContextModel
            {
                NullableEnumParam = SampleEnum.Value2
            }));
            Assert.False(new RuleEngine().CheckCondition("NullableEnumParam == \"Value2\"", new ContextModel
            {
                NullableEnumParam = SampleEnum.Value1
            }));
            Assert.False(new RuleEngine().CheckCondition("NullableEnumParam == \"Value2\"", new ContextModel
            {
                NullableEnumParam = null
            }));
        }

        [Fact]
        public void should_ignore_case()
        {
            Assert.True(new RuleEngine().CheckCondition("StringParam == \"kooboo\"", new ContextModel
            {
                StringParam = "Kooboo"
            }));
        }
    }
}
