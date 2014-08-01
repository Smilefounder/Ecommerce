using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Commerce.Web.Framework.Mvc
{
    public abstract class AddinAreaRegistration : AreaRegistration
    {
        public abstract void RegisterArea(AddinAreaRegistrationContext context);

        public sealed override void RegisterArea(AreaRegistrationContext context)
        {
            RegisterArea(new AddinAreaRegistrationContext(context));
        }
    }
}
