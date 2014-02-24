using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Services
{
    public interface IOrderPaymentService
    {
        Order GetOrderByExternalPaymentTransactionId(string externalPaymentTransactionId, string paymentGatewayName);

        void AcceptPaymentResult(Order order, ProcessPaymentResult result);
    }
}
