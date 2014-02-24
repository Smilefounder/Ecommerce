using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    /// <summary>
    /// Represents a domain specific event.
    /// TODO: 前台的Query event也要显示在后台挂activity，但那些事件不能算是Domain event
    /// </summary>
    /// <remarks>
    /// The purpose of this marker interface is to distinguish domain events (bussiness events) from infrastructure events.
    /// Domain events are open to business experts, so they can be listed in backend to let admins bind activities to them,
    /// while infrastructure events are only open to developers.
    /// </remarks>
    public interface IDomainEvent : IEvent
    {
    }
}
