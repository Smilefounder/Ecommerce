using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Tests.Common.Events
{
    public class EventTrackingScope : IDisposable
    {
        // 'Current' should always has value, because we might have tests which don't need to track event firing.
        // We should ensure these tests won't throw NullReferenceException because the previous test clear the 'Current' host.
        // Every start or dispose should reset the 'Current' value.
        static ThreadLocal<EventTrackingScope> _current = new ThreadLocal<EventTrackingScope>(() => new EventTrackingScope());

        public static EventTrackingScope Current
        {
            get
            {
                return _current.Value;
            }
        }

        public static EventTrackingScope Begin()
        {
            _current.Value = new EventTrackingScope();
            Event.Host = () => EventTrackingScope.Current.Host;
            return Current;
        }

        private Dictionary<Type, int> _eventRaisingTimes = new Dictionary<Type, int>();

        public EventHost Host { get; private set; }

        private EventTrackingScope()
        {
            Host = new EventHost();
            Host.EventRaising += Host_EventRaising;
        }

        public int TotalRaisingTimes<TEvent>()
            where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (_eventRaisingTimes.ContainsKey(eventType))
            {
                return _eventRaisingTimes[eventType];
            }

            return 0;
        }

        void Host_EventRaising(object sender, EventHostEventArgs e)
        {
            var eventType = e.Event.GetType();
            if (!_eventRaisingTimes.ContainsKey(eventType))
            {
                _eventRaisingTimes.Add(eventType, 1);
            }
            else
            {
                _eventRaisingTimes[eventType]++;
            }
        }

        public void Dispose()
        {
            Event.Host = () => EventHost.Instance;
            _current.Value = new EventTrackingScope();
        }
    }
}
