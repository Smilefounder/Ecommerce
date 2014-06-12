using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    /// <summary>
    /// 表示业务事件，用于和技术层面的事件区分开来。
    /// </summary>
    public interface IBusinessEvent : IEvent
    {
    }

    [Serializable]
    public abstract class BusinessEvent : Event, IBusinessEvent
    {
    }
}
