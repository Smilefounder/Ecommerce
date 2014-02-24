using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime.Mvc;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce
{
    [Dependency(typeof(ICommerceInstanceContext), ComponentLifeStyle.InRequestScope)]
    public class HttpCommerceInstanceContext : ICommerceInstanceContext
    {
        public static readonly string CommerceNameKey = "commerceName";

        private ICommerceInstanceManager _commerceInstanceManager;
        private CommerceInstance _currentInstance;

        public CommerceInstance CurrentInstance
        {
            get
            {
                if (_currentInstance == null)
                {
                    var httpContext = HttpContext.Current;
                    if (httpContext == null)
                        throw new InvalidOperationException("Cannot resolve current commerce outside http context.");

                    var commerceName = httpContext.Request.QueryString[CommerceNameKey];

                    if (String.IsNullOrEmpty(commerceName))
                    {
                        commerceName = httpContext.Request.Form[CommerceNameKey];
                    }

                    if (!String.IsNullOrEmpty(commerceName))
                    {
                        _currentInstance = _commerceInstanceManager.OpenInstance(commerceName);
                    }
                }

                return _currentInstance;
            }
        }

        public HttpCommerceInstanceContext(ICommerceInstanceManager commerceInstanceManager)
        {
            Require.NotNull(commerceInstanceManager, "commerceInstanceManager");

            _commerceInstanceManager = commerceInstanceManager;
        }

        public void Dispose()
        {
            if (_currentInstance != null)
            {
                _currentInstance.Dispose();
            }
        }
    }
}