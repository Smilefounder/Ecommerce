using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    /// <summary>
    /// 定义一个事件消息接口。
    /// </summary>
    public interface IEvent
    {
        DateTime TimestampUtc { get; }
    }
}
