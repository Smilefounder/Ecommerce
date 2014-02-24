using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Events
{
    public class UncommittedEventStream : IEnumerable<IEvent>
    {
        private List<IEvent> _events = new List<IEvent>();

        public int Count
        {
            get
            {
                return _events.Count;
            }
        }

        public UncommittedEventStream() { }

        public void Append(IEvent @event)
        {
            Require.NotNull(@event, "event");
            _events.Add(@event);
        }

        public void Clear()
        {
            _events.Clear();
        }

        public IEnumerator<IEvent> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        static readonly ThreadLocal<UncommittedEventStream> _current = new ThreadLocal<UncommittedEventStream>();

        public static UncommittedEventStream Current
        {
            get
            {
                return _current.Value;
            }
            set
            {
                _current.Value = value;
            }
        }
    }
}
