using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Events
{
    public class EventTrackingScope : IDisposable
    {
        private bool _disposed;
        private Scope<EventTrackingScope> _scope;
        private List<IEvent> _pendingEvents = new List<IEvent>();

        public IEnumerable<IEvent> PendingEvents
        {
            get
            {
                return _pendingEvents;
            }
        }

        private EventTrackingScope()
        {
            _scope = Scope.Begin(this);
        }

        public void AppendEvent(IEvent @event)
        {
            Require.NotNull(@event, "event");
            ThrowIfDisposed();
            _pendingEvents.Add(@event);
        }

        public void Clear()
        {
            ThrowIfDisposed();
            _pendingEvents.Clear();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _pendingEvents.Clear();
                _scope.Dispose();
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("EventTrackingScope");
        }

        public static EventTrackingScope Begin()
        {
            return new EventTrackingScope();
        }

        public static EventTrackingScope Current
        {
            get
            {
                return Scope.Current<EventTrackingScope>();
            }
        }
    }
}
