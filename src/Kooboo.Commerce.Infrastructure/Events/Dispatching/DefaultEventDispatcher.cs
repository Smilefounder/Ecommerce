using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events.Registry;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Events.Dispatching
{
    public class DefaultEventDispatcher : IEventDispatcher
    {
        private IEventHandlerRegistry _eventRegistry;
        private IHandlerActivator _handlerActivator = new DefaultHandlerActivator();
        private IHandlerInvoker _handlerInvoker = new DefaultHandlerInvoker();

        public IEventHandlerRegistry EventRegistry
        {
            get
            {
                return _eventRegistry;
            }
        }

        public IHandlerActivator HandlerActivator
        {
            get
            {
                return _handlerActivator;
            }
            set
            {
                _handlerActivator = value;
            }
        }

        public IHandlerInvoker HandlerInvoker
        {
            get
            {
                return _handlerInvoker;
            }
            set
            {
                _handlerInvoker = value;
            }
        }

        public DefaultEventDispatcher(IEventHandlerRegistry handlerRegistry)
        {
            Require.NotNull(handlerRegistry, "handlerRegistry");

            _eventRegistry = handlerRegistry;
        }

        public void Dispatch(IEvent evnt, EventDispatchingContext context)
        {
            Require.NotNull(evnt, "evnt");
            Require.NotNull(context, "context");

            foreach (var method in _eventRegistry.FindHandlers(evnt.GetType()))
            {
                var awaitAttribute = EventHandlerUtil.GetHandlerAttribute<AwaitDbCommitAttribute>(method);

                if (EventHandlerUtil.IsTimeToExecute(awaitAttribute, context))
                {
                    ExecuteHandler(method, evnt, context);
                }
            }
        }

        private void ExecuteHandler(MethodInfo handlerMethod, IEvent evnt, EventDispatchingContext context)
        {
            var handlerType = handlerMethod.ReflectedType;
            object handler = null;

            try
            {
                handler = _handlerActivator.CreateHandlerInstance(handlerType, context);
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Faile to create handler instance, see inner exception for detail. Handler type: " + handlerType + ".", ex);
            }

            try
            {
                _handlerInvoker.Invoke(handler, handlerMethod, evnt, context);
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Faile to execute event handler, see inner exception for detail. Handler type: " + handlerType + ".", ex);
            }
        }
    }
}
