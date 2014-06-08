using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Activities.RetailPriceDiscount
{
    public class AddInAreaRegistration : AreaRegistrationEx
    {
        public override string AreaName
        {
            get
            {
                return Strings.AreaName;
            }
        }

        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context)
        {
            context.MapRoute(
                name: AreaName + "_default",
                url: AreaName + "/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Kooboo.Commerce.Activities.RetailPriceDiscount.Controllers", "Kooboo.Web.Mvc" }
            );

            base.RegisterArea(context);
        }
    }
}