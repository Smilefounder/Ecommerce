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
    public class NotEqualsOperatorFacts
    {
        [Fact]
        public void can_compare_strings()
        {
            Assert.True(new ConditionEvaluator().Evaluate("StringParam != \"MyValue\"", new ContextModel
            {
                StringParam = "MyValue2"
            }));
            Assert.False(new ConditionEvaluator().Evaluate("StringParam != \"MyValue\"", new ContextModel
            {
                StringParam = "MyValue"
            }));
        }
    }
}
