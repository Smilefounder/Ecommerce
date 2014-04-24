using Kooboo.Commerce.Activities;
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
            HighlightedConditionsExpression = new ConditionsExpressionPrettifier().Prettify(rule.ConditionsExpression, System.Type.GetType(rule.EventType, true));
            CreatedAtUtc = rule.CreatedAtUtc;

            Branches.Add(new ActivityRuleBranchModel
            {
                RuleId = Id,
                Branch = RuleBranch.Then,
                AttachedActivities = rule.ThenActivities.Select(x => new AttachedActivityModel(x)).ToList()
            });

            if (rule.Type == RuleType.Normal)
            {
                Branches.Add(new ActivityRuleBranchModel
                {
                    RuleId = Id,
                    Branch = RuleBranch.Else,
                    AttachedActivities = rule.ElseActivities.Select(x => new AttachedActivityModel(x)).ToList()
                });
            }
        }

        public void ForEachAttachedActivity(Action<AttachedActivityModel> action)
        {
            foreach (var branch in Branches)
            {
                foreach (var activity in branch.AttachedActivities)
                {
                    action(activity);
                }
            }
        }
    }

    public class ActivityRuleBranchModel
    {
        public int RuleId { get; set; }

        public RuleBranch Branch { get; set; }

        public List<AttachedActivityModel> AttachedActivities { get; set; }

        public ActivityRuleBranchModel()
        {
            AttachedActivities = new List<AttachedActivityModel>();
        }
    }

    public class AttachedActivityModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ActivityName { get; set; }

        public string ActivityDisplayName { get; set; }

        public bool ActivityAllowAsyncExecution { get; set; }

        public bool IsEnabled { get; set; }

        public int Priority { get; set; }

        [Display(Name = "Enable Async Execution")]
        public bool EnableAsyncExecution { get; set; }

        public int DelayDays { get; set; }

        public int DelayHours { get; set; }

        public int DelayMinutes { get; set; }

        public int DelaySeconds { get; set; }

        public string ConfigUrl { get; set; }

        public int RuleId { get; set; }

        public RuleBranch RuleBranch { get; set; }

        public AttachedActivityModel()
        {
            IsEnabled = true;
        }

        public AttachedActivityModel(AttachedActivity activity)
        {
            Id = activity.Id;
            RuleId = activity.Rule.Id;
            Description = activity.Description;
            ActivityName = activity.ActivityName;
            IsEnabled = activity.IsEnabled;
            Priority = activity.Priority;
            RuleBranch = activity.RuleBranch;
            EnableAsyncExecution = activity.IsAsyncExeuctionEnabled;

            if (activity.AsyncExecutionDelay > 0)
            {
                var delay = TimeSpan.FromSeconds(activity.AsyncExecutionDelay);
                DelayDays = delay.Days;
                DelayHours = delay.Hours;
                DelayMinutes = delay.Minutes;
                DelaySeconds = delay.Seconds;
            }
        }
    }
}