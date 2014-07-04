using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Activities
{
    public abstract class ActivityBase<TEvent> : IActivity
        where TEvent : IEvent
    {
        public abstract string Name { get; }

        public virtual string DisplayName
        {
            get
            {
                return Name;
            }
        }

        public virtual Type ConfigModelType
        {
            get
            {
                return null;
            }
        }

        public virtual bool AllowAsyncExecution
        {
            get
            {
                return false;
            }
        }

        public bool CanBindTo(Type eventType)
        {
            return typeof(TEvent).IsAssignableFrom(eventType);
        }

        public void Execute(Commerce.Events.IEvent @event, ActivityContext context)
        {
            Execute((TEvent)@event, context);
        }

        protected abstract void Execute(TEvent @event, ActivityContext context);
    }
}
