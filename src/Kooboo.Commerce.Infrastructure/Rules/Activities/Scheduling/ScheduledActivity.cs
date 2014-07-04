using Kooboo.Commerce.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Extensions;
using System.Data.Entity.ModelConfiguration;

namespace Kooboo.Commerce.Rules.Activities.Scheduling
{
    public class ScheduledActivity
    {
        public virtual int Id { get; set; }

        /// <summary>
        /// The CLR type of the event.
        /// </summary>
        public virtual string EventType { get; set; }

        /// <summary>
        /// The json serialized event data (payload).
        /// </summary>
        public virtual string EventData { get; set; }

        public virtual string ActivityName { get; set; }

        public virtual string ActivityConfig { get; set; }

        public virtual DateTime ScheduledExecutionTimeUtc { get; set; }

        public virtual DateTime? StartedAtUtc { get; set; }

        public virtual DateTime? CompletedAtUtc { get; set; }

        public virtual ActivityExecutionStatus Status { get; set; }

        public virtual string ErrorMessage { get; set; }

        public virtual string Exception { get; set; }

        public ScheduledActivity() { }

        public ScheduledActivity(IEvent @event, ConfiguredActivity activity)
        {
            EventType = @event.GetType().AssemblyQualifiedNameWithoutVersion();
            EventData = JsonConvert.SerializeObject(@event);
            ActivityName = activity.ActivityName;
            ActivityConfig = activity.Config;
            ScheduledExecutionTimeUtc = DateTime.UtcNow.AddSeconds(activity.AsyncDelay);
        }

        public virtual IEvent LoadEvent()
        {
            return (IEvent)JsonConvert.DeserializeObject(EventData, Type.GetType(EventType));
        }

        public virtual void MarkStarted()
        {
            Status = ActivityExecutionStatus.InProgress;
            StartedAtUtc = DateTime.UtcNow;
        }

        public virtual void MarkFailed(string errorMessage, string errorDetail = null)
        {
            Status = ActivityExecutionStatus.Failed;
            ErrorMessage = errorMessage;
            Exception = errorDetail;
        }

        public virtual void MarkFailed(Exception exception)
        {
            MarkFailed(exception.Message, exception.Print());
        }

        #region Entity Type Configuration

        class ActivityQueueItemMap : EntityTypeConfiguration<ScheduledActivity>
        {
        }

        #endregion
    }
}
