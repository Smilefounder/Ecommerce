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
                if (typeof(ICustomerQuery).IsAssignableFrom(type))
                {
                    manager.Register(typeof(ICustomerQuery), (ICustomerQuery)Activator.CreateInstance(type));
                }
                else if (typeof(IProductQuery).IsAssignableFrom(type))
                {
                    manager.Register(typeof(IProductQuery), (IProductQuery)Activator.CreateInstance(type));
                }
                else if (typeof(IOrderQuery).IsAssignableFrom(type))
                {
                    manager.Register(typeof(IOrderQuery), (IOrderQuery)Activator.CreateInstance(type));
                }
            }
        }
    }
}