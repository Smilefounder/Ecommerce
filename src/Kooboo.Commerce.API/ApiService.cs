using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public static class ApiService
    {
        public static ICommerceAPI Get(string type, ApiContext context)
        {
            var api = EngineContext.Current.Resolve<ICommerceAPI>(type);
            api.Initialize(context);
            return api;
        }
    }
}
