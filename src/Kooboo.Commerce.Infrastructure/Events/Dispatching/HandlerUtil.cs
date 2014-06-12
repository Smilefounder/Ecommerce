using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Events.Dispatching
{
    static class HandlerUtil
    {
        public static IEnumerable<Type> GetHandledEventTypes(Type handlerType)
        {
            foreach (var @interface in handlerType.GetInterfaces())
            {
                if (!@interface.IsGenericType) continue;

                if (@interface.GetGenericTypeDefinition() == typeof(IHandle<>))
                {
                    foreach (var argType in @interface.GetGenericArguments())
                    {
                        yield return argType;
                    }
                }
            }
        }
    }
}
