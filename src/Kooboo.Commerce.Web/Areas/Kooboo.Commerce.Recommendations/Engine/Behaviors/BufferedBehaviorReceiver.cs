using Kooboo.Commerce.Recommendations.Engine.Buffers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public class BufferedBehaviorReceiver : IBehaviorReceiver
    {
        private IBehaviorReceiver _receiver;
        private Buffer<Behavior> _buffer;

        public BufferedBehaviorReceiver(IBehaviorReceiver receiver, int bufferCapacity, TimeSpan flushInterval)
        {
            _receiver = receiver;
            _buffer = new Buffer<Behavior>(bufferCapacity, flushInterval, DoFlush);
        }

        public void OnReceive(IEnumerable<Behavior> behaviors)
        {
            _buffer.Add(behaviors);
        }

        private void DoFlush(IEnumerable<Behavior> behaviors)
        {
            _receiver.OnReceive(behaviors);
        }
    }
}