using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public abstract class RuleModelBase
    {
        public string Type { get; protected set; }

        protected RuleModelBase(string type)
        {
            Type = type;
        }

        public static RuleModelBase FromRule(RuleBase rule)
        {
            if (rule is AlwaysRule)
            {
                return AlwaysRuleModel.FromRule(rule as AlwaysRule);
            }
            if (rule is IfElseRule)
            {
                return IfElseRuleModel.FromRule(rule as IfElseRule);
            }
            if (rule is SwitchCaseRule)
            {
                return SwitchCaseRuleModel.FromRule(rule as SwitchCaseRule);
            }

            throw new NotSupportedException();
        }

        public abstract RuleBase ToRule(EventEntry @event);
    }
}