using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace Kooboo.Commerce.WebAPI
{
    public class WebApiDependencyResolver : IDependencyResolver
    {
        private IEngine _engine;
        private IDependencyResolver _innerResolver;

        public WebApiDependencyResolver(IEngine engine, IDependencyResolver innerResolver)
        {
            _engine = engine;
            _innerResolver = innerResolver;
        }

        public IDependencyScope BeginScope()
        {
            return _innerResolver.BeginScope();
        }

        public object GetService(Type serviceType)
        {
            var service = _engine.TryResolve(serviceType);
            if (service == null)
            {
                service = _innerResolver.GetService(serviceType);
            }
            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var services = _engine.ResolveAll(serviceType);
            if (services == null)
            {
                services = _innerResolver.GetServices(serviceType);
            }
            return services;
        }

        public void Dispose()
        {
            _innerResolver.Dispose();
        }
    }
}