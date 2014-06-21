using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Bootstrapping
{
    public class PromotionPolicyRegistrar : IDependencyRegistrar
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
            foreach (var type in typeFinder.FindClassesOfType<IPromotionPolicy>())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    containerManager.AddComponent(typeof(IPromotionPolicy), type, type.FullName, ComponentLifeStyle.Transient);
                }
            }
        }
    }
}