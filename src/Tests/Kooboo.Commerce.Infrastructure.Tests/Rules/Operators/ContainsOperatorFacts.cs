using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kooboo.Commerce.Infrastructure.Tests.Rules.Operators
{
    public class ContainsOperatorFacts
    {
        [Fact]
        public void can_compare_strings()
        {
            Assert.True(new RuleEngine().CheckCondition("StringParam contains \"kooboo\"", new ContextModel
            {
                StringParam = "kooboo.com"
            }));
            Assert.True(new RuleEngine().CheckCondition("StringParam contains \"kooboo\"", new ContextModel
            {
                StringParam = "thekoobooecommerce"
            }));
            Assert.False(new RuleEngine().CheckCondition("StringParam contains \"kooboo\"", new ContextModel
            {
                StringParam = "koobo cms"
            }));
        }

        [Fact]
        public void should_ignore_case()
        {
            Assert.True(new RuleEngine().CheckCondition("StringParam contains \"kooboo\"", new ContextModel
            {
                StringParam = "The Kooboo Ecommerce"
            }));
        }
    }
}
