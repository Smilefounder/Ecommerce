using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    /// <summary>
    /// 表示领域事件，领域事件指和业务逻辑有关的事件，用于和技术层面的事件区分开来。
    /// </summary>
    public interface IDomainEvent : IEvent
    {
    }

    [Serializable]
    public abstract class DomainEvent : Event, IDomainEvent
    {
    }
}
