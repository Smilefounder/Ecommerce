using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Commerce.Web.Framework.Mvc
{
    public class AddinAreaRegistrationContext
    {
        private AreaRegistrationContext _context;

        public string AreaName
        {
            get { return _context.AreaName; }
        }

        public ICollection<string> Namespaces
        {
            get { return _context.Namespaces; }
        }

        public RouteCollection Routes
        {
            get { return _context.Routes; }
        }

        public AddinAreaRegistrationContext(AreaRegistrationContext context)
        {
            _context = context;
        }

        public Route MapRoute(string name, string url)
        {
            return AddCommerceAddinFlag(_context.MapRoute(name, url));
        }

        public Route MapRoute(string name, string url, object defaults)
        {
            return AddCommerceAddinFlag(_context.MapRoute(name, url, defaults));
        }

        public Route MapRoute(string name, string url, string[] namespaces)
        {
            return AddCommerceAddinFlag(_context.MapRoute(name, url, namespaces));
        }

        public Route MapRoute(string name, string url, object defaults, object constraints)
        {
            return AddCommerceAddinFlag(_context.MapRoute(name, url, defaults, constraints));
        }

        public Route MapRoute(string name, string url, object defaults, string[] namespaces)
        {
            return AddCommerceAddinFlag(_context.MapRoute(name, url, defaults, namespaces));
        }

        public Route MapRoute(string name, string url, object defaults, object constraints, string[] namespaces)
        {
            return AddCommerceAddinFlag(_context.MapRoute(name, url, defaults, constraints, namespaces));
        }

        private Route AddCommerceAddinFlag(Route route)
        {
            route.DataTokens.Add("commerce-addin", true);
            return route;
        }
    }
}
