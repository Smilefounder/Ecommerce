using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class SwitchCaseRuleModel : RuleModelBase
    {
        public string Parameter { get; set; }

        public IList<CaseModel> Cases { get; set; }

        public IList<RuleModelBase> Default { get; set; }

        public SwitchCaseRuleModel()
            : base("SwitchCase")
        {
            Cases = new List<CaseModel>();
            Default = new List<RuleModelBase>();
        }

        public static SwitchCaseRuleModel FromRule(SwitchCaseRule rule)
        {
            var model = new SwitchCaseRuleModel
            {
                Parameter = rule.Parameter.Name
            };

            foreach (var caze in rule.Cases)
            {
                model.Cases.Add(new CaseModel
                {
                    Value = caze.Key.ToString(),
                    Rules = caze.Value.Select(r => RuleModelBase.FromRule(r)).ToList()
                });
            }

            foreach (var defaultRule in rule.Default)
            {
                model.Default.Add(RuleModelBase.FromRule(defaultRule));
            }

            return model;
        }

        public override RuleBase ToRule(EventEntry @event)
        {
            var param = RuleParameterProviders.Providers.GetParameter(@event.EventType, Parameter);
            var rule = new SwitchCaseRule(param);

            foreach (var caze in Cases)
            {
                var value = Convert.ChangeType(caze.Value, param.ValueType);
                rule.Cases.Add(value, caze.Rules.Select(r => r.ToRule(@event)).ToList());
            }

            foreach (var defaultRule in Default)
            {
                rule.Default.Add(defaultRule.ToRule(@event));
            }

            return rule;
        }
    }
}