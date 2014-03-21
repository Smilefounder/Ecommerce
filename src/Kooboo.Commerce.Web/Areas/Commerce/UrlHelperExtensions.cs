using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web
{
    public static class UrlHelperExtensions
    {
        public static PaymentUrlHelper Payment(this UrlHelper helper)
        {
            return new PaymentUrlHelper(helper);
        }
    }
}