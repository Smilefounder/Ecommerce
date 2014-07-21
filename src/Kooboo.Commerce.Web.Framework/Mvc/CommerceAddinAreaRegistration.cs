using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Commerce.Web.Framework.Mvc
{
    public abstract class CommerceAddinAreaRegistration
    {
        public abstract string AreaName { get; }

        public abstract void RegisterArea(CommerceAddinAreaRegistrationContext context);
    }
}
