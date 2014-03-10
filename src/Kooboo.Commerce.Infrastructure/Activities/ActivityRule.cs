using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class ActivityRule : AggregateRoot
    {
        public virtual int Id { get; set; }

        public virtual string EventType { get; set; }

        public virtual string ConditionsExpression { get; set; }

        public virtual int Sequence { get; set; }

        public virtual ICollection<AttachedActivity> AttachedActivities { get; protected set; }

        public virtual DateTime CreatedAtUtc { get; set; }

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
            var attachedActivity = new AttachedActivity
            {
                Description = description,
                ActivityName = activityName,
                ActivityData = activityData,
                IsEnabled = true
            };

            AttachedActivities.Add(attachedActivity);

            return attachedActivity;
        }

        public bool DetacheActivity(int attachedActivityId)
        {
            var attachedActivity = AttachedActivities.FirstOrDefault(x => x.Id == attachedActivityId);
            if (attachedActivity != null)
            {
                return AttachedActivities.Remove(attachedActivity);
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

        public AttachedActivity()
        {
            CreatedAtUtc = DateTime.UtcNow;
        }
    }
}
