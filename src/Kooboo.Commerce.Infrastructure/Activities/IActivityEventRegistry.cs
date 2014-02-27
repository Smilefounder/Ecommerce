using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public interface IActivityEventRegistry
    {
        IEnumerable<string> GetCategories();

        IEnumerable<Type> GetEventTypesByCategory(string category);

        void RegisterEvents(IEnumerable<Type> eventTypes);

        void RegisterAssemblies(params Assembly[] assemblies);

        void RegisterAssemblies(IEnumerable<Assembly> assemblies);
    }
}
