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
    public class LessThanOrEqualOperatorFacts
    {
        [Fact]
        public void can_compare_integers()
        {
            Assert.True(new ConditionEvaluator().Evaluate("Int32Param <= 5", new ContextModel
            {
                Int32Param = 5
            }));
            Assert.True(new ConditionEvaluator().Evaluate("Int32Param <= 5", new ContextModel
            {
                Int32Param = 4
            }));
            Assert.False(new ConditionEvaluator().Evaluate("Int32Param <= 5", new ContextModel
            {
                Int32Param = 6
            }));
        }
    }
}
