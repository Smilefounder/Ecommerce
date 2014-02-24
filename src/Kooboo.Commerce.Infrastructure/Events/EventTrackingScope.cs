using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Transactions;

namespace Kooboo.Commerce.Events
{
    public class EventTrackingScope : IDisposable
    {
        private UncommittedEventStream _parentStream;
        private UncommittedEventStream _currentStream;
        private IEventDispatcher _eventDispatcher;

        public bool IsCompleted { get; private set; }

        public EventTrackingScope(IEventDispatcher eventDispatcher)
            : this(eventDispatcher, EventTrackingScopeOption.Required)
        {
        }

        public EventTrackingScope(IEventDispatcher eventDispatcher, EventTrackingScopeOption scopeOption)
        {
            Require.NotNull(eventDispatcher, "eventDispatcher");

            _eventDispatcher = eventDispatcher;

            if (scopeOption == EventTrackingScopeOption.Required)
            {
                var currentEventStream = UncommittedEventStream.Current ?? new UncommittedEventStream();
                SetCurrentEventStream(currentEventStream);
            }
            else if (scopeOption == EventTrackingScopeOption.RequiresNew)
            {
                SetCurrentEventStream(new UncommittedEventStream());
            }
            // if scopeOption is Suppress, nothing need to be done
        }

        public void Complete()
        {
            if (IsCompleted)
                throw new InvalidOperationException("Current event scope has already completed.");

            IsCompleted = true;
        }

        public void Dispose()
        {
            try
            {
                if (IsCompleted)
                {
                    DispatchPendingEvents();
                }
                else
                {
                    ClearPendingEvents();
                }
            }
            finally
            {
                RestoreEventStream();
            }
        }

        private void SetCurrentEventStream(UncommittedEventStream currentStream)
        {
            _parentStream = UncommittedEventStream.Current;
            UncommittedEventStream.Current = currentStream;
            _currentStream = currentStream;
        }

        private void RestoreEventStream()
        {
            var currentStream = UncommittedEventStream.Current;

            if (currentStream != _currentStream)
                throw new InvalidOperationException("Event scope are incorrectly nested. Ensure any nested event scope is disposed before the disposing of parent event scope.");

            if (currentStream != _parentStream)
            {
                UncommittedEventStream.Current = _parentStream;
            }

            _currentStream = null;
        }

        private void DispatchPendingEvents()
        {
            var currentStream = _currentStream;
            if (currentStream != null)
            {
                var events = currentStream.ToList();

                currentStream.Clear();

                var context = new EventDispatchingContext(EventDispatchingPhase.OnTransactionCommitted, true);

                foreach (var @event in events)
                {
                    _eventDispatcher.Dispatch(@event, context);
                }
            }
        }

        private void ClearPendingEvents()
        {
            var currentStream = _currentStream;
            if (currentStream != null)
            {
                currentStream.Clear();
            }
        }
    }
}
