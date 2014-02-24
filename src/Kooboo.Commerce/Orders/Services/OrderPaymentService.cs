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

        public void AcceptPaymentResult(Order order, ProcessPaymentResult result)
        {
            if (order.PaymentStatus != PaymentStatus.Success)
            {
                if (result.PaymentStatus == PaymentStatus.Success)
                {
                    order.PaymentStatus = PaymentStatus.Success;
                    order.PaymentCompletedAtUtc = DateTime.UtcNow;

                    Event.Apply(new OrderPaid(order));
                }
                else
                {
                    order.PaymentStatus = result.PaymentStatus;
                }
            }
        }
    }
}
