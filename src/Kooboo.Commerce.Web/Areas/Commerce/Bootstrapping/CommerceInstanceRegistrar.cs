using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Infrastructure.Dependencies
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
            containerManager.AddComponent(typeof(ICurrentInstanceProvider), typeof(ThreadScopeCurrentInstanceProvider), "ThreadScopeCurrentInstanceProvider", ComponentLifeStyle.Singleton);
            containerManager.AddComponent(typeof(ICurrentInstanceProvider), typeof(HttpCurrentInstanceProvider), "HttpCurrentInstanceProvider", ComponentLifeStyle.Transient);

            var container = ((Kooboo.CMS.Common.Runtime.Dependency.Ninject.ContainerManager)containerManager).Container;

            container.Bind<ICommerceDatabase>()
                     .ToMethod(ctx =>
                     {
                         var instance = CommerceInstance.Current;
                         if (instance != null)
                         {
                             return instance.Database;
                         }

                         return null;
                     });

            container.Bind(typeof(IRepository<>)).To(typeof(CommerceRepository<>));
        }
    }
}