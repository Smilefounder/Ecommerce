using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Bootstrapping
{
    public class PaymentProcessorRegistrar : IDependencyRegistrar
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
            foreach (var type in typeFinder.FindClassesOfType<IPaymentProcessor>())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    containerManager.AddComponent(typeof(IPaymentProcessor), type, type.FullName, ComponentLifeStyle.Transient);
                }
            }
        }
    }
}