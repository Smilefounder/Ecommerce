using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events.Dispatching;
using Kooboo.Commerce.Events.Registry;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace Kooboo.Commerce.Infrastructure.Dependencies
{
    public class EventDependencyRegistrar : IDependencyRegistrar
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

            // Event Registry
            var eventRegistry = new DefaultEventHandlerRegistry();
            eventRegistry.RegisterAssemblies(assemblies);
            containerManager.AddComponentInstance<IEventHandlerRegistry>(eventRegistry);

            var activityEventRegistry = new DefaultActivityEventRegistry();
            activityEventRegistry.RegisterAssemblies(assemblies);
            containerManager.AddComponentInstance<IActivityEventRegistry>(activityEventRegistry);

            // Event Dispatcher
            var eventDispatcher = new DefaultEventDispatcher(eventRegistry);
            containerManager.AddComponentInstance<IEventDispatcher>(eventDispatcher);
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