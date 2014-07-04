using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kooboo.Commerce.Infrastructure.Tests.Rules.Operators
{
    public class NotContainsOperatorFacts
    {
        [Fact]
        public void can_compare_strings()
        {
            Assert.True(new ConditionEvaluator().Evaluate("StringParam not contains \"kooboo\"", new ContextModel
            {
                StringParam = "koobo is incorrect"
            }));
        }

        [Fact]
        public void should_ignore_case()
        {
            Assert.False(new ConditionEvaluator().Evaluate("StringParam not contains \"kooboo\"", new ContextModel
            {
                StringParam = "Kooboo in Xiamen"
            }));
        }
    }
}
