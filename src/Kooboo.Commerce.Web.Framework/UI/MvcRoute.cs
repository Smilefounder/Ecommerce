using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI
{
    public class MvcRoute
    {
        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public IDictionary<string, object> RouteValues { get; set; }

        public bool Matches(ControllerContext controllerContext)
        {
            if (Area != null)
            {
                var area = controllerContext.RouteData.DataTokens["area"] as string;
                if (area != Area)
                {
                    return false;
                }
            }

            if (Controller != null)
            {
                var controller = controllerContext.RouteData.Values["controller"] as string;
                if (controller != Controller)
                {
                    return false;
                }
            }

            if (Action != null)
            {
                var action = controllerContext.RouteData.Values["action"] as string;
                if (action != Action)
                {
                    return false;
                }
            }

            if (RouteValues != null && RouteValues.Count > 0)
            {
                foreach (var each in RouteValues)
                {
                    var value = controllerContext.RouteData.Values[each.Key];
                    if (value == null || !value.Equals(each.Value))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
