using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Commerce.Web.Areas.Commerce.Bootstrapping
{
    public class AddinAreaRegistrar : IDependencyRegistrar
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
            foreach (var type in typeFinder.FindClassesOfType<CommerceAddinAreaRegistration>())
            {
                var registration = Activator.CreateInstance(type) as CommerceAddinAreaRegistration;
                var context = new AreaRegistrationContext(registration.AreaName, RouteTable.Routes);

                string registrationNamespace = registration.GetType().Namespace;
                if (registrationNamespace != null)
                {
                    context.Namespaces.Add(registrationNamespace + ".*");
                }

                registration.RegisterArea(new CommerceAddinAreaRegistrationContext(context));
            }
        }
    }
}