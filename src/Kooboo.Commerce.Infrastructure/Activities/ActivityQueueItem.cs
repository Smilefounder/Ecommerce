using Kooboo.Commerce.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Extensions;
using System.Data.Entity.ModelConfiguration;
using Kooboo.Commerce.Rules.Activities;

namespace Kooboo.Commerce.Activities
{
    public enum QueueItemStatus
    {
        Pending = 0,
        InProgress = 1,
        Failed = 2,
        Success = 3
    }

    public class ActivityQueueItem
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

        public virtual DateTime ScheduledExecuteTimeUtc { get; set; }

        public virtual DateTime? StartedAtUtc { get; set; }

        public virtual DateTime? CompletedAtUtc { get; set; }

        public virtual QueueItemStatus Status { get; set; }

        public virtual string ErrorMessage { get; set; }

        public virtual string Exception { get; set; }

        public ActivityQueueItem() { }

        public ActivityQueueItem(IEvent @event, ConfiguredActivity activity)
        {
            EventType = @event.GetType().AssemblyQualifiedNameWithoutVersion();
            EventData = JsonConvert.SerializeObject(@event);
            ActivityName = activity.ActivityName;
            ActivityConfig = activity.Config;
            ScheduledExecuteTimeUtc = DateTime.UtcNow.AddSeconds(activity.AsyncDelay);
        }

        public virtual IEvent LoadEvent()
        {
            return (IEvent)JsonConvert.DeserializeObject(EventData, Type.GetType(EventType));
        }

        public virtual void MarkStarted()
        {
            Status = QueueItemStatus.InProgress;
            StartedAtUtc = DateTime.UtcNow;
        }

        public virtual void MarkFailed(string errorMessage, string errorDetail = null)
        {
            Status = QueueItemStatus.Failed;
            ErrorMessage = errorMessage;
            Exception = errorDetail;
        }

        public virtual void MarkFailed(Exception exception)
        {
            MarkFailed(exception.Message, exception.Print());
        }

        #region Entity Type Configuration

        class ActivityQueueItemMap : EntityTypeConfiguration<ActivityQueueItem>
        {
        }

        #endregion
    }
}
