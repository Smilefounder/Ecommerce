using Kooboo.Commerce.Rules;
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
            Assert.True(new RuleEngine().CheckCondition("Int32Param <= 5", new ContextModel
            {
                Int32Param = 5
            }));
            Assert.True(new RuleEngine().CheckCondition("Int32Param <= 5", new ContextModel
            {
                Int32Param = 4
            }));
            Assert.False(new RuleEngine().CheckCondition("Int32Param <= 5", new ContextModel
            {
                Int32Param = 6
            }));
        }
    }
}
