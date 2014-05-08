using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Kooboo.Commerce.Web
{
    public static class RouteValues
    {
        public static RouteValueDictionary From(NameValueCollection nv)
        {
            var routeValues = new RouteValueDictionary();

            foreach (var key in nv.AllKeys)
            {
                if (string.IsNullOrEmpty(key))
                    continue;
                routeValues.Add(key, nv[key]);
            }

            return routeValues;
        }
    }
}
