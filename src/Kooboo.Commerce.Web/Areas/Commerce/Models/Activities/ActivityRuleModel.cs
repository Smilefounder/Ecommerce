using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules.Expressions.Formatting;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;
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

        public string ConditionsExpression { get; set; }

        public string HighlightedConditionsExpression { get; set; }

        public List<ActivityRuleBranchModel> Branches { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public ActivityRuleModel()
        {
            Branches = new List<ActivityRuleBranchModel>();
        }

        public ActivityRuleModel(ActivityRule rule)
            : this()
        {
            Id = rule.Id;
            Type = rule.Type;
            EventType = rule.EventType;
            ConditionsExpression = rule.ConditionsExpression;
            HighlightedConditionsExpression = new HtmlExpressionFormatter().Format(rule.ConditionsExpression, System.Type.GetType(rule.EventType, true));
            CreatedAtUtc = rule.CreatedAtUtc;

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