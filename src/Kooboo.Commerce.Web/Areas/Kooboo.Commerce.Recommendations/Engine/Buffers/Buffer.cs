using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Buffers
{
    public class Buffer<T>
    {
        private readonly object _lock = new object();
        private ICollection<T> _items;
        private Func<ICollection<T>> _collectionFactory;
        private Action<IEnumerable<T>> _flushAction;
        private Timer _timer;

        public int Capacity { get; private set; }

        public Buffer(int capacity, TimeSpan flushInterval, Action<IEnumerable<T>> flushAction)
            : this(capacity, flushInterval, flushAction, () => new List<T>())
        {
        }

        public Buffer(int capacity, TimeSpan flushInterval, Action<IEnumerable<T>> flushAction, Func<ICollection<T>> collectionFactory)
        {
            Capacity = capacity;
            _flushAction = flushAction;
            _collectionFactory = collectionFactory;
            _timer = new Timer(OnTimerTicked, null, flushInterval, flushInterval);
        }

        private void OnTimerTicked(object state)
        {
            Flush();
        }

        public void Add(IEnumerable<T> items)
        {
            lock (_lock)
            {
                if (_items == null)
                {
                    _items = _collectionFactory();
                    foreach (var item in items)
                    {
                        _items.Add(item);
                    }
                }
            }
        }

        public void Flush()
        {
            ICollection<T> items;

            lock (_lock)
            {
                items = _items;
                _items = null;
            }

            if (items != null)
            {
                _flushAction(items);
            }
        }
    }
}