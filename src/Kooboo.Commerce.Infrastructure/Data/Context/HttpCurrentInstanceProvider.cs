using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Data.Context
{
    public class HttpCurrentInstanceProvider : ICurrentInstanceProvider
    {
        public Func<HttpContextBase> GetHttpContext = () => new HttpContextWrapper(HttpContext.Current);

        private ICommerceInstanceManager _instanceManager;

        public HttpCurrentInstanceProvider(ICommerceInstanceManager instanceManager)
        {
            _instanceManager = instanceManager;
        }

        public CommerceInstance GetCurrentInstance()
        {
            var httpContext = GetHttpContext();

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

        static string GetInstanceName(HttpContextBase httpContext)
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
