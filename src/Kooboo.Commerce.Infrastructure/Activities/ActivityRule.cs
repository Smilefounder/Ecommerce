using Kooboo.Commerce.Activities.Events;
using Kooboo.Commerce.Events;
using Kooboo.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public enum RuleBranch
    {
        Then,
        Else,
    }

    public class ActivityRule
    {
        public int Id { get; set; }

        [Required, StringLength(500)]
        public string EventType { get; set; }

        public RuleType Type { get; set; }

        [StringLength(3000)]
        public string ConditionsExpression { get; set; }

        public virtual ICollection<AttachedActivity> AttachedActivities { get; protected set; }

        [NotMapped]
        public virtual ICollection<AttachedActivity> ThenActivities
        {
            get
            {
                return AttachedActivities.Where(x => x.RuleBranch == RuleBranch.Then).ToList();
            }
        }

        [NotMapped]
        public virtual ICollection<AttachedActivity> ElseActivities
        {
            get
            {
                return AttachedActivities.Where(x => x.RuleBranch == RuleBranch.Else).ToList();
            }
        }

        public DateTime CreatedAtUtc { get; set; }

        protected ActivityRule()
        {
            CreatedAtUtc = DateTime.UtcNow;
            AttachedActivities = new List<AttachedActivity>();
        }

        public static ActivityRule Create(Type eventType, string conditionsExpression, RuleType type)
        {
            return new ActivityRule
            {
                EventType = eventType.AssemblyQualifiedNameWithoutVersion(),
                ConditionsExpression = conditionsExpression,
                Type = type
            };
        }

        public AttachedActivity AttachActivity(RuleBranch branch, string description, string activityName, string activityData)
        {
            var attachedActivity = new AttachedActivity(this, branch, description, activityName, activityData);
            AttachedActivities.Add(attachedActivity);

            Event.Raise(new ActivityAttached(this, attachedActivity));

            return attachedActivity;
        }

        public bool DetachActivity(int attachedActivityId)
        {
            var attachedActivity = AttachedActivities.ById(attachedActivityId);
            if (attachedActivity != null)
            {
                return DetachActivity(attachedActivity);
            }

            return false;
        }

        public bool DetachActivity(AttachedActivity activity)
        {
            Require.NotNull(activity, "activity");

            var detached = AttachedActivities.Remove(activity);
            if (detached)
            {
                Event.Raise(new ActivityDetached(this, activity));
            }

            return detached;
        }

        public void DetachAllActivities()
        {
            foreach (var activity in AttachedActivities.ToList())
            {
                DetachActivity(activity);
            }
        }
    }

    public enum RuleType
    {
        Normal = 0,
        Always = 1
    }
}
