using Kooboo.Commerce.Rules;
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

        public static RuleModelBase CreateFrom(RuleBase rule)
        {
            if (rule is AlwaysRule)
            {
                return CreateAlways(rule as AlwaysRule);
            }
            if (rule is IfElseRule)
            {
                return CreateIfElse(rule as IfElseRule);
            }

            throw new NotSupportedException();
        }

        static AlwaysRuleModel CreateAlways(AlwaysRule rule)
        {
            var model = new AlwaysRuleModel();
            foreach (var activity in rule.Activities)
            {
                model.Activities.Add(ConfiguredActivityModel.CreateFrom(activity));
            }

            return model;
        }

        static IfElseRuleModel CreateIfElse(IfElseRule rule)
        {
            var model = new IfElseRuleModel();

            foreach (var thenRule in rule.Then)
            {
                model.Then.Add(CreateFrom(thenRule));
            }

            foreach (var elseRule in rule.Else)
            {
                model.Else.Add(CreateFrom(elseRule));
            }

            return model;
        }
    }
}