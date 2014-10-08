using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    /// <summary>
    /// 用于事件注册、执行的容器。
    /// </summary>
    /// <remarks>
    /// 每个容器都互相隔离，以方便单元测试，运行时只会有一个容器存在。本类为内部使用，普通开发人员应使用 Event 上的方法。
    /// </remarks>
    public class EventHost
    {
        public static readonly EventHost Instance = new EventHost();

        private readonly Dictionary<Type, List<object>> _handlersByEvents = new Dictionary<Type, List<object>>();

        public void Raise<TEvent>(TEvent @event, EventContext context)
            where TEvent : IEvent
        {
            Require.NotNull(@event, "event");
            Require.NotNull(context, "context");

            var handlers = GetEventHandlers(@event.GetType());
            foreach (var handler in handlers)
            {
                ExecuteEventHandler<TEvent>(handler, @event, context);
            }
        }

        public void Listen(Type eventType, Type handlerType)
        {
            Require.NotNull(eventType, "eventType");
            Require.NotNull(handlerType, "handlerType");

            DoListen(eventType, (object)handlerType);
        }

        public void Listen<TEvent>(Type handlerType)
            where TEvent : IEvent
        {
            Listen(typeof(TEvent), handlerType);
        }

        public void Listen<TEvent>(IHandle<TEvent> handler)
            where TEvent : IEvent
        {
            Require.NotNull(handler, "handler");
            DoListen(typeof(TEvent), handler);
        }

        public void Listen(Type eventType, Action<IEvent, EventContext> handler)
        {
            DoListen(eventType, handler);
        }

        public void Listen<TEvent>(Action<TEvent, EventContext> handler)
            where TEvent : IEvent
        {
            Require.NotNull(handler, "handler");
            DoListen(typeof(TEvent), handler);
        }

        private void DoListen(Type eventType, object handler)
        {
            List<object> handlers;
            if (!_handlersByEvents.TryGetValue(eventType, out handlers))
            {
                handlers = new List<object>();
                _handlersByEvents.Add(eventType, handlers);
            }

            handlers.Add(handler);
        }

        private IEnumerable<object> GetEventHandlers(Type eventType)
        {
            var handlers = GetDirectEventHandlers(eventType).ToList();

            // Here we need to support base event subscribtion:
            // If event A is raised, handlers subscribing to A and A's base events all need to be invoked.
            var baseEventType = eventType.BaseType;
            while (baseEventType != null && baseEventType != typeof(object))
            {
                handlers.AddRange(GetDirectEventHandlers(baseEventType));
                baseEventType = baseEventType.BaseType;
            }

            foreach (var @interface in eventType.GetInterfaces())
            {
                if (typeof(IEvent).IsAssignableFrom(@interface))
                {
                    handlers.AddRange(GetDirectEventHandlers(@interface));
                }
            }

            return handlers;
        }

        private IEnumerable<object> GetDirectEventHandlers(Type eventType)
        {
            List<object> handlers;
            if (_handlersByEvents.TryGetValue(eventType, out handlers))
            {
                return handlers;
            }

            return Enumerable.Empty<object>();
        }

        private void ExecuteEventHandler<TEvent>(object handler, TEvent @event, EventContext context)
             where TEvent : IEvent
        {
            var handlerInstance = ResolveEventHandlerInstance<TEvent>(handler);

            try
            {
                (handlerInstance as IHandle<TEvent>).Handle(@event, context);
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Error executing event handler, see inner exception for detail. Handler type: " + handlerInstance.GetType() + ".", ex);
            }
        }

        private object ResolveEventHandlerInstance<TEvent>(object handler)
            where TEvent : IEvent
        {
            // 'handler' might be a Type
            var handlerType = handler as Type;
            if (handlerType != null)
            {
                object handlerInstance = null;

                try
                {
                    handlerInstance = TypeActivator.CreateInstance(handler as Type);
                }
                catch (Exception ex)
                {
                    throw new EventHandlerException("Error creating event handler instance, see inner exception for detail. Handler type: " + (handler as Type) + ".", ex);
                }

                if (handlerInstance == null)
                {
                    throw new EventHandlerException("Error creating event handler instance.");
                }

                return handlerInstance;
            }

            // 'handler' might be an Action<TEvent>
            var action = handler as Action<TEvent, EventContext>;
            if (action != null)
            {
                return new RelayEventHandler<TEvent>(action);
            }

            // 'handler' might be an IHandle<TEvent> instance
            return handler;
        }
    }
}
