using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public abstract class Rule
    {
        public abstract IEnumerable<ConfiguredActivity> Execute(object dataContext);

        protected IEnumerable<ConfiguredActivity> Further(IEnumerable<Rule> rules, object dataContext)
        {
            var result = new List<ConfiguredActivity>();
            foreach (var rule in rules)
            {
                result.AddRange(rule.Execute(dataContext));
            }

            return result;
        }
    }
}
