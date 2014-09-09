using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kooboo.Commerce.Web.Bootstrapping
{
    public class EventHandlerRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 100;
            }
        }

        static readonly string[] _ignoredAssemblyPrefixes = new[] 
        {
            "EntityFramework", "Kooboo.CMS.", "System.", "Microsoft.", "DotNetOpenAuth."
        };

        public void Register(IContainerManager containerManager, CMS.Common.Runtime.ITypeFinder typeFinder)
        {
            var assemblies = typeFinder.GetAssemblies()
                                       .Where(x => !IsIgnoredAssembly(x))
                                       .ToList();

            foreach (var assembly in assemblies)
            {
                foreach (var handlerType in assembly.GetTypes())
                {
                    if (!handlerType.IsClass || handlerType.IsAbstract || handlerType.IsGenericType)
                    {
                        continue;
                    }

                    foreach (var eventType in GetHandledEventTypes(handlerType))
                    {
                        Event.Listen(eventType, handlerType);
                    }
                }
            }
        }

        static IEnumerable<Type> GetHandledEventTypes(Type handlerType)
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

        static bool IsIgnoredAssembly(Assembly assembly)
        {
            foreach (var prefix in _ignoredAssemblyPrefixes)
            {
                if (assembly.FullName.StartsWith(prefix))
                {
                    return true;
                }
            }

            return false;
        }
    }
}