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

        IEnumerable<Type> AllEventTypes();

        IEnumerable<Type> FindEventTypesByCategory(string category);

        void RegisterEvents(IEnumerable<Type> eventTypes);

        void RegisterAssemblies(params Assembly[] assemblies);

        void RegisterAssemblies(IEnumerable<Assembly> assemblies);
    }
}
