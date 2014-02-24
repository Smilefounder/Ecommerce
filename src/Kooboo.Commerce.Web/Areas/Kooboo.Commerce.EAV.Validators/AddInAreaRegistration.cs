using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.EAV.Validators
{
    public class AddInAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return Strings.AreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: AreaName + "_default",
                url: AreaName + "/{controller}/{action}",
                namespaces: new[] { "Kooboo.Commerce.EAV.Validators.Controllers" }
            );
        }
    }
}