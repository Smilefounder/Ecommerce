using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    public interface IEvent
    {
        DateTime TimestampUtc { get; }
    }
}
