using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Payments;
using System;

namespace Kooboo.Commerce.Orders
{
    class OrderingProcessManager
        : IHandle<PaymentStatusChanged>
    {
        private OrderService _orderService;
        private PaymentService _paymentService;

        public OrderingProcessManager(OrderService orderService, PaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
        }

        public void Handle(PaymentStatusChanged @event, CommerceInstance instance)
        {
            var payment = _paymentService.Find(@event.PaymentId);

            if (@event.NewStatus != PaymentStatus.Success)
            {
                return;
            }

            var orderId = @payment.OrderId;
            var order = _orderService.Find(orderId);
            _orderService.AcceptPayment(order, payment);
        }
    }
}
