using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.Plugins
{
    static class HttpContextExtensions
    {
        public static string CurrentCartSessionId(this HttpContextBase context)
        {
            return context.Session.SessionID;
        }
    }
}
