using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Data.Context
{
    public class HttpCurrentInstanceProvider : ICurrentInstanceProvider
    {
        public Func<HttpContextBase> GetHttpContext = () =>
        {
            var httpContext = HttpContext.Current;
            return httpContext == null ? null : new HttpContextWrapper(httpContext);
        };

        private ICommerceInstanceManager _instanceManager;

        public HttpCurrentInstanceProvider(ICommerceInstanceManager instanceManager)
        {
            _instanceManager = instanceManager;
        }

        public CommerceInstance GetCurrentInstance()
        {
            var httpContext = GetHttpContext();
            if (httpContext == null)
            {
                return null;
            }

            var instance = httpContext.Items["Kooboo.Commerce.Data.CommerceInstance.Current"] as CommerceInstance;
            if (instance == null)
            {
                var instanceName = GetInstanceName(GetHttpContext());
                if (!String.IsNullOrWhiteSpace(instanceName))
                {
                    instance = _instanceManager.GetInstance(instanceName);
                    httpContext.Items["Kooboo.Commerce.Data.CommerceInstance.Current"] = instance;
                }
            }

            return instance;
        }

        protected virtual string GetInstanceName(HttpContextBase httpContext)
        {
            var instanceName = httpContext.Items["instance"] as string;

            if (String.IsNullOrWhiteSpace(instanceName))
            {
                instanceName = httpContext.Request.QueryString["instance"];
            }

            if (String.IsNullOrWhiteSpace(instanceName))
            {
                instanceName = httpContext.Request.Form["instance"];
            }

            if (String.IsNullOrWhiteSpace(instanceName))
            {
                instanceName = httpContext.Request.Headers["X-Kooboo-Commerce-Instance"];
            }

            if (String.IsNullOrWhiteSpace(instanceName))
            {
                instanceName = httpContext.Request.RequestContext.RouteData.Values["instance"] as string;
            }

            return instanceName;
        }
    }
}
