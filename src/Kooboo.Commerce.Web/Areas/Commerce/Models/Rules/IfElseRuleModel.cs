using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class IfElseRuleModel : RuleModelBase
    {
        public IList<Condition> Conditions { get; set; }

        public IList<RuleModelBase> Then { get; set; }

        public IList<RuleModelBase> Else { get; set; }

        public IfElseRuleModel()
            : base("IfElse")
        {
            Conditions = new List<Condition>();
            Then = new List<RuleModelBase>();
            Else = new List<RuleModelBase>();
        }

        public static IfElseRuleModel FromRule(IfElseRule rule)
        {
            var model = new IfElseRuleModel();

            model.Conditions = rule.Conditions.ToList();

            foreach (var thenRule in rule.Then)
            {
                model.Then.Add(FromRule(thenRule));
            }

            foreach (var elseRule in rule.Else)
            {
                model.Else.Add(FromRule(elseRule));
            }

            return model;
        }

        public override Rule ToRule(EventSlot slot)
        {
            var rule = new IfElseRule();
            rule.Conditions = Conditions.ToList();

            foreach (var thenRuleModel in Then)
            {
                rule.Then.Add(thenRuleModel.ToRule(slot));
            }

            foreach (var elseRuleModel in Else)
            {
                rule.Else.Add(elseRuleModel.ToRule(slot));
            }

            return rule;
        }
    }
}