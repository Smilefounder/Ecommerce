using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Areas.Commerce.Menu;
using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Bootstrapping
{
    public class MenuInjectionRegistrar : IDependencyRegistrar
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
            containerManager.AddComponent(typeof(IMenuInjection), typeof(GlobalMenuInjection), "GlobalMenuInjection", ComponentLifeStyle.Singleton);
            containerManager.AddComponent(typeof(IMenuInjection), typeof(InstanceMenuInjection), "InstanceMenuInjection", ComponentLifeStyle.Singleton);

            foreach (var type in typeFinder.FindClassesOfType<IMenuInjection>())
            {
                if (typeof(CommerceMenuInjection).IsAssignableFrom(type)
                    && type != typeof(GlobalMenuInjection)
                    && type != typeof(InstanceMenuInjection))
                {
                    containerManager.AddComponent(typeof(IMenuInjection), type, type.FullName, ComponentLifeStyle.Transient);
                }
            }
        }
    }
}