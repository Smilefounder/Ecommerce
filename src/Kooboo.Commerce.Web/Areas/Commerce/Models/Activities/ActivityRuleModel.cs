using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions;
using Kooboo.Commerce.Rules.Expressions.Formatting;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class ActivityRuleModel
    {
        public int Id { get; set; }

        public RuleType Type { get; set; }

        public string EventType { get; set; }

        public List<Condition> Conditions { get; set; }

        public string HighlightedConditionsExpression { get; set; }

        public List<ActivityRuleBranchModel> Branches { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public ActivityRuleModel()
        {
            Conditions = new List<Condition>();
            Branches = new List<ActivityRuleBranchModel>();
        }

        public ActivityRuleModel(ActivityRule rule)
            : this()
        {
            Id = rule.Id;
            Type = rule.Type;
            EventType = rule.EventType;
            CreatedAtUtc = rule.CreatedAtUtc;
            Conditions = rule.Conditions.ToList();

            Branches.Add(new ActivityRuleBranchModel
            {
                RuleId = Id,
                Branch = RuleBranch.Then,
                AttachedActivities = rule.ThenActivityInfos.Select(x => new AttachedActivityModel(x)).ToList()
            });

            if (rule.Type == RuleType.Normal)
            {
                Branches.Add(new ActivityRuleBranchModel
                {
                    RuleId = Id,
                    Branch = RuleBranch.Else,
                    AttachedActivities = rule.ElseActivityInfos.Select(x => new AttachedActivityModel(x)).ToList()
                });
            }
        }
    }
}