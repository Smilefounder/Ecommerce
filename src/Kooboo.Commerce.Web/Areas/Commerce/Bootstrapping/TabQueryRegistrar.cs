using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Bootstrapping
{
    public class TabQueryRegistrar : IDependencyRegistrar
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
            foreach (var type in typeFinder.FindClassesOfType(typeof(ITabQuery)))
            {
                containerManager.AddComponent(typeof(ITabQuery), type, type.FullName, ComponentLifeStyle.Transient);
            }
        }
    }
}