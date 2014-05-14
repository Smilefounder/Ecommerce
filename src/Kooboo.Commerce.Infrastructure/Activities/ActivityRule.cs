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
    public class ActivityRule
    {
        public int Id { get; set; }

        [Required, StringLength(500)]
        public string EventType { get; set; }

        public RuleType Type { get; set; }

        [StringLength(3000)]
        public string ConditionsExpression { get; set; }

        public virtual ICollection<AttachedActivityInfo> AttachedActivityInfos { get; protected set; }

        [NotMapped]
        public virtual ICollection<AttachedActivityInfo> ThenActivityInfos
        {
            get
            {
                return AttachedActivityInfos.Where(x => x.RuleBranch == RuleBranch.Then).ToList();
            }
        }

        [NotMapped]
        public virtual ICollection<AttachedActivityInfo> ElseActivityInfos
        {
            get
            {
                return AttachedActivityInfos.Where(x => x.RuleBranch == RuleBranch.Else).ToList();
            }
        }

        public DateTime CreatedAtUtc { get; set; }

        protected ActivityRule() { }

        public ActivityRule(Type eventType, string conditionsExpression, RuleType type)
        {
            EventType = eventType.AssemblyQualifiedNameWithoutVersion();
            ConditionsExpression = conditionsExpression;
            Type = type;
            CreatedAtUtc = DateTime.UtcNow;
            AttachedActivityInfos = new List<AttachedActivityInfo>();
        }

        public AttachedActivityInfo AttachActivity(RuleBranch branch, string description, string activityName, object config = null)
        {
            var attachedActivity = new AttachedActivityInfo(this, branch, description, activityName, config);
            AttachedActivityInfos.Add(attachedActivity);

            Event.Raise(new ActivityAttached(this, attachedActivity));

            return attachedActivity;
        }

        public bool DetachActivity(int attachedActivityInfoId)
        {
            var attachedActivity = AttachedActivityInfos.Find(attachedActivityInfoId);
            if (attachedActivity != null)
            {
                return DetachActivity(attachedActivity);
            }

            return false;
        }

        public bool DetachActivity(AttachedActivityInfo attachedActivityInfo)
        {
            Require.NotNull(attachedActivityInfo, "attachedActivityInfo");

            var detached = AttachedActivityInfos.Remove(attachedActivityInfo);
            if (detached)
            {
                Event.Raise(new ActivityDetached(this, attachedActivityInfo));
            }

            return detached;
        }

        public void DetachAllActivities()
        {
            foreach (var activity in AttachedActivityInfos.ToList())
            {
                DetachActivity(activity);
            }
        }
    }
}
