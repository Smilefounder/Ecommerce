using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Bootstrapping
{
    public class CommerceDataSourceRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 200;
            }
        }

        public void Register(IContainerManager containerManager, CMS.Common.Runtime.ITypeFinder typeFinder)
        {
            foreach (var type in typeFinder.FindClassesOfType<CommerceDataSource>())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    containerManager.AddComponent(typeof(CommerceDataSource), type, type.FullName, ComponentLifeStyle.Transient);
                }
            }
        }
    }
}