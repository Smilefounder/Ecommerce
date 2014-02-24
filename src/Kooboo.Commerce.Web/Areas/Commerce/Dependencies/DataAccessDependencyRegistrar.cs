using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Dependencies
{
    public class DataAccessDependencyRegistrar : IDependencyRegistrar
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

            container.Bind<ICommerceDatabase>()
                     .ToMethod(ctx =>
                     {
                         var commerceContext = (ICommerceInstanceContext)ctx.Kernel.GetService(typeof(ICommerceInstanceContext));
                         if (commerceContext.CurrentInstance == null)
                             throw new InvalidOperationException("Cannot resolve commerce instance from current context.");

                         return commerceContext.CurrentInstance.Database;
                     });

            container.Bind(typeof(IRepository<>))
                     .To(typeof(CommerceRepository<>));
        }
    }
}