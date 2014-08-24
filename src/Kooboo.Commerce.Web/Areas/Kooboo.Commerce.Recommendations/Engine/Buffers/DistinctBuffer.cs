using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Buffers
{
    public class DistinctBuffer<T> : Buffer<T>
    {
        public DistinctBuffer(int capacity, TimeSpan flushInterval, Action<IEnumerable<T>> flushAction)
            : this(capacity, flushInterval, flushAction, EqualityComparer<T>.Default)
        {
        }

        public DistinctBuffer(int capacity, TimeSpan flushInterval, Action<IEnumerable<T>> flushAction, IEqualityComparer<T> comparer)
            : base(capacity, flushInterval, flushAction, () => new HashSet<T>(comparer))
        {
        }
    }
}