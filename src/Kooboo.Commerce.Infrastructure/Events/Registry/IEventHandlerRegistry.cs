using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kooboo.Commerce.Events.Registry
{
    public interface IEventHandlerRegistry
    {
        IEnumerable<MethodInfo> FindHandlers(Type eventType);

        void RegisterAssemblies(params Assembly[] assemblies);

        void RegisterAssemblies(IEnumerable<Assembly> assemblies);
    }
}
