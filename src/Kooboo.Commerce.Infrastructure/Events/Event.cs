using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using Kooboo.CMS.Common.Runtime;
using System.Reflection;

namespace Kooboo.Commerce.Events
{
    [Serializable]
    public abstract class Event : IEvent
    {
        public DateTime TimestampUtc { get; set; }

        protected Event()
        {
            TimestampUtc = DateTime.UtcNow;
        }

        public static void Raise(IEvent @event)
        {
            Require.NotNull(@event, "event");

            var manager = EventHandlerManager.Instance;
            if (manager == null)
                throw new InvalidOperationException("Cannot retrieve instance of " + typeof(EventHandlerManager) + ".");

            var handleMethods = manager.FindHandlers(@event.GetType());
            foreach (var handleMethod in handleMethods)
            {
                ExecuteHandler(handleMethod, @event);
            }
        }

        static void ExecuteHandler(MethodInfo handleMethod, IEvent @event)
        {
            var handlerType = handleMethod.ReflectedType;
            object handler = null;

            try
            {
                handler = CreateEventHandler(handlerType);
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Error creating event handler instance, see inner exception for detail. Handler type: " + handlerType + ".", ex);
            }

            if (handler == null)
            {
                throw new EventHandlerException("Error creating event handler instance.");
            }

            try
            {
                handleMethod.Invoke(handler, new[] { @event });
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Error executing event handler, see inner exception for detail. Handler type: " + handlerType + ".", ex);
            }
        }

        static object CreateEventHandler(Type type)
        {
            return EngineContext.Current.Resolve(type);
        }
    }
}
