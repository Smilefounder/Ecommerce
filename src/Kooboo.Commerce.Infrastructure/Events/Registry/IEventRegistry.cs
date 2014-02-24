using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kooboo.Commerce.Events.Registry
{
    public interface IEventRegistry
    {
        IEnumerable<string> AllEventCategories();

        IEnumerable<Type> AllEvents();

        IEnumerable<Type> FindUncategorizedEvents();

        IEnumerable<Type> FindEventsByCategory(string category);

        IEnumerable<MethodInfo> FindHandlerMethods(Type eventType);

        void RegisterAssemblies(params Assembly[] assemblies);

        void RegisterAssemblies(IEnumerable<Assembly> assemblies);

        void Clear();
    }
}
