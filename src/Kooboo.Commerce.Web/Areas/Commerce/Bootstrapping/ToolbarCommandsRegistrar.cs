using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Framework.UI.Toolbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Bootstrapping
{
    public class ToolbarCommandsRegistrar : IDependencyRegistrar
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
            foreach (var type in typeFinder.FindClassesOfType<IToolbarCommand>())
            {
                containerManager.AddComponent(typeof(IToolbarCommand), type, type.FullName, ComponentLifeStyle.Transient);
            }
        }
    }
}