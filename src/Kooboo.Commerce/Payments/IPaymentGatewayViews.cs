using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments
{
    public interface IPaymentGatewayViews
    {
        string PaymentGatewayName { get; }

        RedirectToRouteResult Settings(PaymentMethod method, ControllerContext context);
    }
}
