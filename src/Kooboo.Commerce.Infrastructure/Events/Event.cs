using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using Kooboo.CMS.Common.Runtime;
using System.Reflection;
using System.Linq;

namespace Kooboo.Commerce.Events
{
    public static class Event
    {
        /// <summary>
        /// 返回当前 EventHost 实例的委托，用于单元测试。
        /// </summary>
        public static Func<EventHost> Host = () => EventHost.Instance;

        public static void Raise<TEvent>(TEvent @event, EventContext context)
            where TEvent : IEvent
        {
            Host().Raise<TEvent>(@event, context);
        }

        public static void Listen(Type eventType, Type handlerType)
        {
            Host().Listen(eventType, handlerType);
        }

        public static void Listen<TEvent>(Type handlerType)
            where TEvent : IEvent
        {
            Host().Listen<TEvent>(handlerType);
        }

        public static void Listen<TEvent>(IHandle<TEvent> handler)
            where TEvent : IEvent
        {
            Host().Listen<TEvent>(handler);
        }

        public static void Listen(Type eventType, Action<IEvent, EventContext> handler)
        {
            Host().Listen(eventType, handler);
        }

        public static void Listen<TEvent>(Action<TEvent, EventContext> handler)
            where TEvent : IEvent
        {
            Host().Listen<TEvent>(handler);
        }
    }
}
