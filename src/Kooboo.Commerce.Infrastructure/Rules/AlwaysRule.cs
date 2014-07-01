using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class AlwaysRule : RuleBase
    {
        private List<ConfiguredActivity> _activities = new List<ConfiguredActivity>();

        public IList<ConfiguredActivity> Activities
        {
            get
            {
                return _activities;
            }
        }

        public override IEnumerable<ConfiguredActivity> Execute(object dataContext)
        {
            return Activities.ToList();
        }
    }
}
