using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Context;
using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Data.Initialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Bootstrapping
{
    public class CommerceInstanceRegistrar : IDependencyRegistrar
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
            // Current instance providers
            containerManager.AddComponent(typeof(ICurrentInstanceProvider), typeof(ThreadScopeCurrentInstanceProvider), "ThreadScopeCurrentInstanceProvider", ComponentLifeStyle.Singleton);
            containerManager.AddComponent(typeof(ICurrentInstanceProvider), typeof(SiteAwareHttpCurrentInstanceProvider), "SiteAwareHttpCurrentInstanceProvider", ComponentLifeStyle.Transient);

            // Data folder factory
            containerManager.AddComponentInstance<DataFolderFactory>(DataFolderFactory.Current);

            // Instance intializers
            foreach (var type in typeFinder.FindClassesOfType<IInstanceInitializer>())
            {
                containerManager.AddComponent(typeof(IInstanceInitializer), type, type.FullName, ComponentLifeStyle.Transient);
            }

            // Repository and ICommerceDatabase
            var container = ((Kooboo.CMS.Common.Runtime.Dependency.Ninject.ContainerManager)containerManager).Container;

            container.Bind<CommerceInstance>()
                     .ToMethod(ctx => CommerceInstance.Current);

            container.Bind<ICommerceDatabase>()
                     .ToMethod(ctx =>
                     {
                         var instance = CommerceInstance.Current;
                         if (instance == null)
                             throw new InvalidOperationException("Cannot resolve ICommerceDatabase, because there's no commerce instance in the context.");

                         return instance.Database;
                     });

            container.Bind(typeof(IRepository<>)).To(typeof(CommerceRepository<>));
        }
    }
}