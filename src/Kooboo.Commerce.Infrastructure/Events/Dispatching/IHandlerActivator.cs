using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Dispatching
{
    public interface IHandlerActivator
    {
        object CreateHandlerInstance(Type handlerType, EventDispatchingContext dispatchingContext);
    }

    public class DefaultHandlerActivator : IHandlerActivator
    {
        private Func<IEngine> _engineAccessor;

        public DefaultHandlerActivator()
            : this(() => EngineContext.Current) { }

        public DefaultHandlerActivator(Func<IEngine> engineAccessor)
        {
            Require.NotNull(engineAccessor, "engineAccessor");
            _engineAccessor = engineAccessor;
        }

        public object CreateHandlerInstance(Type handlerType, EventDispatchingContext dispatchingContext)
        {
            try
            {
                var engine = _engineAccessor();
                var handler = engine.Resolve(handlerType);
                OnHandlerInstanceCreated(handler, dispatchingContext);
                return handler;
            }
            catch (Exception ex)
            {
                throw new EventHandlerException("Failed creating event handler instance. Handler type: " + handlerType + ".", ex);
            }
        }

        protected virtual void OnHandlerInstanceCreated(object handler, EventDispatchingContext dispatchingContext) { }
    }
}
