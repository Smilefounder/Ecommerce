using Kooboo.Commerce.Recommendations.Engine.Buffers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public class BufferedBehaviorObserver : IBehaviorObserver
    {
        private IBehaviorObserver _observer;
        private Buffer<Behavior> _buffer;

        public BufferedBehaviorObserver(IBehaviorObserver observer, int bufferCapacity, TimeSpan flushInterval)
        {
            _observer = observer;
            _buffer = new Buffer<Behavior>(bufferCapacity, flushInterval, DoFlush);
        }

        public void OnReceive(IEnumerable<Behavior> behaviors)
        {
            _buffer.Add(behaviors);
        }

        private void DoFlush(IEnumerable<Behavior> behaviors)
        {
            _observer.OnReceive(behaviors);
        }
    }
}