using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration
{
    [Dependency(typeof(IShoppingCartSessionIdProvider))]
    public class DefaultShoppingCartSessionIdProvider : IShoppingCartSessionIdProvider
    {
        public string GetCurrentSessionId(bool ensure)
        {
            return HttpContext.Current.Session.SessionID;
        }
    }
}
