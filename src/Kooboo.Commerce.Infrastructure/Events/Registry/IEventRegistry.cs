using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Events.Registry
{
    public interface IEventRegistry
    {
        IEnumerable<string> AllCategories();

        IEnumerable<EventRegistrationEntry> AllEvents();

        IEnumerable<EventRegistrationEntry> FindByCategory(string category);

        EventRegistrationEntry FindByType(Type eventType);

        void RegisterEvents(IEnumerable<Type> eventTypes);

        void RegisterAssemblies(params Assembly[] assemblies);

        void RegisterAssemblies(IEnumerable<Assembly> assemblies);
    }
}
