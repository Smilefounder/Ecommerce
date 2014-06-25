using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Registry;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kooboo.Commerce.Infrastructure.Dependencies
{
    public class EventRegistrar : IDependencyRegistrar
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

            // Handler Registry
            EventHandlerManager.Instance.RegisterAssemblies(assemblies);

            // Event Registry
            var eventRegistry = new DefaultEventRegistry();
            eventRegistry.RegisterAssemblies(assemblies);
            containerManager.AddComponentInstance<IEventRegistry>(eventRegistry);
        }

        private bool IsIgnoredAssembly(Assembly assembly)
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