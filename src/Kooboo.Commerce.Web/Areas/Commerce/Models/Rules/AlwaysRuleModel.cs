using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class AlwaysRuleModel : RuleModelBase
    {
        public IList<ConfiguredActivityModel> Activities { get; set; }

        public AlwaysRuleModel()
            : base("Always")
        {
            Activities = new List<ConfiguredActivityModel>();
        }

        public static AlwaysRuleModel FromRule(AlwaysRule rule)
        {
            var model = new AlwaysRuleModel();
            foreach (var activity in rule.Activities)
            {
                model.Activities.Add(ConfiguredActivityModel.FromConfiguredActivity(activity));
            }

            return model;
        }

        public override Rule ToRule(EventEntry @event)
        {
            var rule = new AlwaysRule();
            foreach (var activityModel in Activities)
            {
                rule.Activities.Add(activityModel.ToConfiguredActivity());
            }
            return rule;
        }
    }
}