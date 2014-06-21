using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Bootstrapping
{
    public class ActivityRegistrar : IDependencyRegistrar
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
            foreach (var activityType in typeFinder.FindClassesOfType<IActivity>())
            {
                if (activityType.IsClass && !activityType.IsAbstract)
                {
                    containerManager.AddComponent(typeof(IActivity), activityType, activityType.FullName, ComponentLifeStyle.Transient);
                }
            }
        }
    }
}