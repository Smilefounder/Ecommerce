using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Bootstrapping
{
    public class ExtendedQueryRegistrar : IDependencyRegistrar
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
            foreach (var type in typeFinder.FindClassesOfType<ICustomerExtendedQuery>())
            {
                containerManager.AddComponent(typeof(ICustomerExtendedQuery), type, type.FullName, ComponentLifeStyle.Transient);
            }

            foreach (var type in typeFinder.FindClassesOfType<IOrderExtendedQuery>())
            {
                containerManager.AddComponent(typeof(IOrderExtendedQuery), type, type.FullName, ComponentLifeStyle.Transient);
            }

            foreach (var type in typeFinder.FindClassesOfType<IProductExtendedQuery>())
            {
                containerManager.AddComponent(typeof(IProductExtendedQuery), type, type.FullName, ComponentLifeStyle.Transient);
            }
        }
    }
}