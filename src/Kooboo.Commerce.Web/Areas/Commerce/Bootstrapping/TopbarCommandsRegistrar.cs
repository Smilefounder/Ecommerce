using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Bootstrapping
{
    public class TopbarCommandsRegistrar : IDependencyRegistrar
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
            foreach (var type in typeFinder.FindClassesOfType<ITopbarCommand>())
            {
                containerManager.AddComponent(typeof(ITopbarCommand), type, type.FullName, ComponentLifeStyle.Transient);
            }
        }
    }
}