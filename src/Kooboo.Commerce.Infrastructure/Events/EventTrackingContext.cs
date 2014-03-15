using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Events
{
    public class EventTrackingContext : IDisposable
    {
        private List<IEvent> _pendingEvents = new List<IEvent>();
        private IEventDispatcher _dispatcher;

        public IEnumerable<IEvent> PendingEvents
        {
            get
            {
                return _pendingEvents;
            }
        }

        private EventTrackingContext() { }

        public void AppendEvent(IEvent @event)
        {
            Require.NotNull(@event, "event");
            _pendingEvents.Add(@event);
        }

        public void Clear()
        {
            _pendingEvents.Clear();
        }

        public void Dispose()
        {
            _pendingEvents.Clear();
            Unbind();
        }

        public static EventTrackingContext Begin()
        {
            var context = new EventTrackingContext();
            Bind(context);
            return context;
        }

        static readonly ThreadLocal<Stack<EventTrackingContext>> _contexts = new ThreadLocal<Stack<EventTrackingContext>>(() => new Stack<EventTrackingContext>());

        public static EventTrackingContext Current
        {
            get
            {
                var stack = _contexts.Value;
                return stack.Count == 0 ? null : stack.Peek();
            }
        }

        static void Bind(EventTrackingContext context)
        {
            Require.NotNull(context, "context");
            _contexts.Value.Push(context);
        }

        static void Unbind()
        {
            var stack = _contexts.Value;
            if (stack.Count == 0)
                throw new InvalidOperationException("No EventTrackingContext was binded.");

            stack.Pop();
        }
    }
}
