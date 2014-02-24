using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Events.Dispatching
{
    static class EventHandlerUtil
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

        public static TAttribute GetHandlerAttribute<TAttribute>(MethodInfo handlerMethod)
            where TAttribute : Attribute
        {
            var attribute = handlerMethod.GetCustomAttributes(typeof(TAttribute), true).OfType<TAttribute>().FirstOrDefault();

            if (attribute == null)
            {
                attribute = handlerMethod.ReflectedType.GetCustomAttributes(typeof(TAttribute), true).OfType<TAttribute>().FirstOrDefault();
            }

            return attribute;
        }

        public static bool IsTimeToExecute(AwaitTransactionCompleteAttribute foundAwaitAttribute, EventDispatchingContext dispatchingContext)
        {
            if (dispatchingContext.Phase == EventDispatchingPhase.OnEventRaised)
            {
                // If no AwaitTransactionCommitted attribute defined,
                // then the handler is expected to execute immediately on event raised
                if (foundAwaitAttribute == null)
                {
                    return true;
                }

                if (!dispatchingContext.IsInEventTrackingScope)
                {
                    // If the handler is expected to execute on transaction committed but it's not in transaction,
                    // then check if the handler is specified WhenNotInTranaction.ExecuteImmediately,
                    // if yes, then we execute the handler, otherwise ignore this handler
                    if (foundAwaitAttribute.WhenNotInTransaction == WhenNoInTransaction.ExecuteImmediately)
                    {
                        return true;
                    }
                }
            }
            else // Phase == EventDispatchingPhase.OnTransactionCommitted
            {
                // In OnTransactionCommitted phase, we only execute handlers attributed with AwaitTransactionCommitted
                if (foundAwaitAttribute != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
