using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events.Registry;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Events.Dispatching
{
    public class DefaultEventDispatcher : IEventDispatcher
    {
        public IEventHandlerRegistry HandlerRegistry { get; private set; }

        public Func<Type, object> ActivateEventHandler = type => EngineContext.Current.Resolve(type);

        public DefaultEventDispatcher(IEventHandlerRegistry handlerRegistry)
        {
            Require.NotNull(handlerRegistry, "handlerRegistry");

            HandlerRegistry = handlerRegistry;
        }

        public void Dispatch(IEvent evnt)
        {
            Require.NotNull(evnt, "evnt");

            foreach (var method in HandlerRegistry.FindHandlers(evnt.GetType()))
            {
                ExecuteHandler(method, evnt);
            }
        }

        private void ExecuteHandler(MethodInfo handlerMethod, IEvent evnt)
        {
            var handlerType = handlerMethod.ReflectedType;
            object handler = null;

            try
            {
                handler = ActivateEventHandler(handlerType);
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Failed to create handler instance, see inner exception for detail. Handler type: " + handlerType + ".", ex);
            }

            if (handler == null)
            {
                throw new EventHandlerException("Failed to create handler instance.");
            }

            try
            {
                handlerMethod.Invoke(handler, new[] { evnt });
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Faile to execute event handler, see inner exception for detail. Handler type: " + handlerType + ".", ex);
            }
        }
    }
}
