using Kooboo.Commerce.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Extensions;

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

        public virtual int RuleId { get; set; }

        public virtual int AttachedActivityId { get; set; }

        public virtual string EventType { get; set; }

        public virtual string EventPayload { get; set; }

        public virtual DateTime ScheduledExecuteTimeUtc { get; set; }

        public virtual DateTime? StartedAtUtc { get; set; }

        public virtual DateTime? CompletedAtUtc { get; set; }

        public virtual QueueItemStatus Status { get; set; }

        public virtual string ErrorMessage { get; set; }

        public virtual string Exception { get; set; }

        public ActivityQueueItem() { }

        public ActivityQueueItem(AttachedActivity activity, IEvent @event)
        {
            RuleId = activity.Rule.Id;
            AttachedActivityId = activity.Id;
            EventType = @event.GetType().AssemblyQualifiedNameWithoutVersion();
            EventPayload = JsonConvert.SerializeObject(@event);
            ScheduledExecuteTimeUtc = activity.CalculateExecutionTime(@event.TimestampUtc);
        }

        public virtual IEvent LoadEvent()
        {
            return (IEvent)JsonConvert.DeserializeObject(EventPayload, Type.GetType(EventType));
        }

        public virtual void MarkStarted()
        {
            Status = QueueItemStatus.InProgress;
            StartedAtUtc = DateTime.UtcNow;
        }

        public virtual void MarkFailed(Exception exception)
        {
            Status = QueueItemStatus.Failed;
            ErrorMessage = exception.Message;
            Exception = exception.Print();
        }
    }
}
