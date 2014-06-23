using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce
{
    public static class ControllerContextExtensions
    {
        public static string AreaName(this ControllerContext controllerContext)
        {
            return controllerContext.RouteData.DataTokens["area"] as string;
        }

        public static string ControllerName(this ControllerContext controllerContext)
        {
            return controllerContext.RouteData.Values["controller"] as string;
        }

        public static string ActionName(this ControllerContext controllerContext)
        {
            return controllerContext.RouteData.Values["action"] as string;
        }
    }
}
