using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments
{
    public interface IPaymentProcessorViews
    {
        string PaymentProcessorName { get; }

        RedirectToRouteResult Settings(PaymentMethod method, ControllerContext context);
    }
}
