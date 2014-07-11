using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Bootstrapping
{
    public class QueryRegistrar : IDependencyRegistrar
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
            var manager = QueryManager.Instance;

            foreach (var type in typeFinder.FindClassesOfType(typeof(IQuery)))
            {
                manager.Register(Activator.CreateInstance(type) as IQuery);
            }
        }
    }
}