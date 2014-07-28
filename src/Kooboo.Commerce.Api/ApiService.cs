using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public static class ApiService
    {
        public static ICommerceApi Get(string type, ApiContext context)
        {
            var api = EngineContext.Current.Resolve<ICommerceApi>(type);
            api.Initialize(context);
            return api;
        }
    }
}
