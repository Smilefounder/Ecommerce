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
            foreach (var type in typeFinder.FindClassesOfType<ExtendedQuery.CustomerQuery>())
            {
                containerManager.AddComponent(typeof(ExtendedQuery.CustomerQuery), type, type.FullName, ComponentLifeStyle.Transient);
            }

            foreach (var type in typeFinder.FindClassesOfType<ExtendedQuery.OrderQuery>())
            {
                containerManager.AddComponent(typeof(ExtendedQuery.OrderQuery), type, type.FullName, ComponentLifeStyle.Transient);
            }

            foreach (var type in typeFinder.FindClassesOfType<ExtendedQuery.ProductQuery>())
            {
                containerManager.AddComponent(typeof(ExtendedQuery.ProductQuery), type, type.FullName, ComponentLifeStyle.Transient);
            }
        }
    }
}