using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class ActivityRuleModel
    {
        public int Id { get; set; }

        public string EventType { get; set; }

        public string ConditionsExpression { get; set; }

        public string HighlightedConditionsExpression { get; set; }

        public int Sequence { get; set; }

        public List<AttachedActivityModel> AttachedActivities { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public ActivityRuleModel()
        {
            AttachedActivities = new List<AttachedActivityModel>();
        }

        public ActivityRuleModel(ActivityRule rule)
            : this()
        {
            Id = rule.Id;
            EventType = rule.EventType;
            ConditionsExpression = rule.ConditionsExpression;
            HighlightedConditionsExpression = new ConditionsExpressionHumanizer().Humanize(rule.ConditionsExpression);
            Sequence = rule.Sequence;
            CreatedAtUtc = rule.CreatedAtUtc;

            foreach (var attachedActivity in rule.AttachedActivities)
            {
                AttachedActivities.Add(new AttachedActivityModel(attachedActivity));
            }
        }
    }

    public class AttachedActivityModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ActivityName { get; set; }

        public string ActivityDisplayName { get; set; }

        public bool IsEnabled { get; set; }

        public int Priority { get; set; }

        public string ConfigUrl { get; set; }

        public int RuleId { get; set; }

        public AttachedActivityModel() { }

        public AttachedActivityModel(AttachedActivity activity)
        {
            Id = activity.Id;
            RuleId = activity.Rule.Id;
            Description = activity.Description;
            ActivityName = activity.ActivityName;
            IsEnabled = activity.IsEnabled;
            Priority = activity.Priority;
        }
    }
}