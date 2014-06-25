using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Infrastructure.Dependencies
{
    public class DataAccessRegistrar : IDependencyRegistrar
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
            var container = ((Kooboo.CMS.Common.Runtime.Dependency.Ninject.ContainerManager)containerManager).Container;

            container.Bind<ICommerceInstanceNameResolver>().ToMethod(ctx =>
            {
                var resolver = new CompositeCommerceInstanceNameResolver
                (
                    new HttpContextItemCommerceInstanceNameResolver(),
                    new QueryStringCommerceInstanceNameResolver(),
                    new HttpHeaderCommerceInstanceNameResolver("X-Kooboo-Commerce-Instance"),
                    new PostParamsCommerceInstanceNameResolver(),
                    new RouteDataCommerceInstanceResolver("instance")
                );

                return resolver;
            });

            container.Bind<ICommerceDatabase>()
                     .ToMethod(ctx =>
                     {
                         var context = ctx.Kernel.GetService(typeof(CommerceInstanceContext)) as CommerceInstanceContext;
                         if (context.CurrentInstance == null)
                             throw new InvalidOperationException("Cannot resolve current commerce instance. Ensure 'commerceName' parameter is included in the http request.");

                         return context.CurrentInstance.Database;
                     });

            container.Bind(typeof(IRepository<>)).To(typeof(CommerceRepository<>));
        }
    }
}