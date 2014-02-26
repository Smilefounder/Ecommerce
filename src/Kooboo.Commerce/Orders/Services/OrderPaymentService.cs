using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Services
{
    [Dependency(typeof(IOrderPaymentService))]
    public class OrderPaymentService : IOrderPaymentService
    {
        private IRepository<Order> _orderRepository;

        public OrderPaymentService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order GetOrderByExternalPaymentTransactionId(string externalPaymentTransactionId, string paymentGatewayName)
        {
            return _orderRepository.Get(x => x.ExternalPaymentTransactionId == externalPaymentTransactionId && x.PaymentGatewayName == paymentGatewayName);
        }

        public void HandlePaymentResult(Order order, ProcessPaymentResult result)
        {
            Require.NotNull(order, "order");
            Require.NotNull(result, "result");

            // Ignore if already succeeded
            if (order.PaymentStatus != PaymentStatus.Success)
            {
                if (result.PaymentStatus == PaymentStatus.Success)
                {
                    order.MarkPaymentSucceeded(result.PaymentTransactionId);
                }
                else if (result.PaymentStatus == PaymentStatus.Cancelled)
                {
                    order.MarkPaymentCancelled();
                }
                else if (result.PaymentStatus == PaymentStatus.Failed)
                {
                    order.MarkPaymentFailed();
                }

                // Ignore pending status because it's the initial status
            }
        }
    }
}
