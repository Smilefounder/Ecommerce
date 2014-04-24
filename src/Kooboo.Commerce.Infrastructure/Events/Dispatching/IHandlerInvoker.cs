using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Events.Dispatching
{
    public interface IHandlerInvoker
    {
        void Invoke(object handler, MethodInfo handleMethod, IEvent evnt);
    }

    public class DefaultHandlerInvoker : IHandlerInvoker
    {
        public void Invoke(object handler, MethodInfo handleMethod, IEvent evnt)
        {
            handleMethod.Invoke(handler, new object[] { evnt });
        }
    }
}
