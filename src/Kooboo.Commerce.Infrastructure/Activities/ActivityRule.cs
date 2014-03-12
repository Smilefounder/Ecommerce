using Kooboo.Commerce.Activities.Events;
using Kooboo.Commerce.Events;
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

        public string ConditionsExpression { get; set; }

        public int Sequence { get; set; }

        public virtual ICollection<AttachedActivity> AttachedActivities { get; protected set; }

        public DateTime CreatedAtUtc { get; set; }

        protected ActivityRule()
        {
            CreatedAtUtc = DateTime.UtcNow;
            AttachedActivities = new List<AttachedActivity>();
        }

        public static ActivityRule Create(Type eventType, string conditionsExpression)
        {
            return new ActivityRule
            {
                EventType = eventType.GetVersionUnawareAssemblyQualifiedName(),
                ConditionsExpression = conditionsExpression
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

    public class AttachedActivity
    {
        public virtual int Id { get; set; }

        public virtual string Description { get; set; }

        public virtual string ActivityName { get; set; }

        public virtual string ActivityData { get; set; }

        public virtual bool IsEnabled { get; set; }

        public virtual int Priority { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }

        public virtual ActivityRule Rule { get; set; }

        protected AttachedActivity()
        {
        }

        public AttachedActivity(ActivityRule rule)
        {
            Rule = rule;
            CreatedAtUtc = DateTime.UtcNow;
        }
    }
}
