using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.AuthorizeNet
{
    [Dependency(typeof(IPaymentProcessorViews), Key = "Kooboo.Commerce.Payments.AuthorizeNetPaymentProcessorViews")]
    public class AuthorizeNetPaymentProcessorViews : IPaymentProcessorViews
    {
        public string PaymentProcessorName
        {
            get
            {
                return Strings.PaymentGatewayName;
            }
        }

        public System.Web.Mvc.RedirectToRouteResult Settings(PaymentMethod method, System.Web.Mvc.ControllerContext context)
        {
            return Routes.RedirectToAction("Settings", "Home", new { methodId = method.Id, area = Strings.AreaName });
        }
    }
}