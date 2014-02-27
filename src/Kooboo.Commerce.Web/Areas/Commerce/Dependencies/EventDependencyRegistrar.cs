using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events.Dispatching;
using Kooboo.Commerce.Events.Registry;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace Kooboo.Commerce.Web.Areas.Commerce.Dependencies
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

        public void Register(IContainerManager containerManager, CMS.Common.Runtime.ITypeFinder typeFinder)
        {
            var assemblies = typeFinder.GetAssemblies();// BuildManager.GetReferencedAssemblies().OfType<Assembly>().ToList();

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
    }
}