using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Bootstrapping
{
    public class CommerceDbProviderRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 1000;
            }
        }

        public void Register(IContainerManager containerManager, CMS.Common.Runtime.ITypeFinder typeFinder)
        {
            foreach (var type in typeFinder.FindClassesOfType<ICommerceDbProvider>())
            {
                CommerceDbProviders.Providers.Add((ICommerceDbProvider)containerManager.Resolve(type));
            }
        }
    }
}