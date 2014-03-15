using Kooboo.Commerce.Activities.Events;
using Kooboo.Commerce.Events;
using Kooboo.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class ActivityRule : AggregateRoot
    {
        public int Id { get; set; }

        public string EventType { get; set; }

        public RuleType Type { get; set; }

        public string ConditionsExpression { get; set; }

        public virtual ICollection<AttachedActivity> AttachedActivities { get; protected set; }

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

        public AttachedActivity AttacheActivity(string description, string activityName, string activityData)
        {
            var attachedActivity = new AttachedActivity(this)
            {
                Description = description,
                ActivityName = activityName,
                ActivityData = activityData,
                IsEnabled = true
            };

            AttachedActivities.Add(attachedActivity);

            Event.Apply(new ActivityAttached(this, attachedActivity));

            return attachedActivity;
        }

        public AttachedActivity FindAttachedActivity(int attachedActivityId)
        {
            return AttachedActivities.FirstOrDefault(x => x.Id == attachedActivityId);
        }

        public bool DetacheActivity(int attachedActivityId)
        {
            var attachedActivity = AttachedActivities.FirstOrDefault(x => x.Id == attachedActivityId);
            if (attachedActivity != null)
            {
                AttachedActivities.Remove(attachedActivity);
                Event.Apply(new ActivityDetached(this, attachedActivity));
                return true;
            }

            return false;
        }
    }

    public enum RuleType
    {
        Normal = 0,
        Always = 1
    }
}
