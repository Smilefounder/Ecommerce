using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.iDeal
{
    [Dependency(typeof(IPaymentGatewayViews), Key = "Kooboo.Commerce.Payments.iDeal.PaymentGatewayViews")]
    public class IDealPaymentGatewayViews : IPaymentGatewayViews
    {
        public string PaymentGatewayName
        {
            get
            {
                return Strings.PaymentGatewayName;
            }
        }

        public System.Web.Mvc.RedirectToRouteResult Settings(PaymentMethod method, System.Web.Mvc.ControllerContext context)
        {
            return Routes.RedirectToAction(
                "Settings", "Home", new { methodId = method.Id, area = Strings.AreaName });
        }
    }
}