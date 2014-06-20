using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;
using System;

namespace Kooboo.Commerce.Orders
{
    public class OrderingProcessManager
        : IHandle<PaymentStatusChanged>
    {
        private IOrderService _orderService;
        private IPaymentService _paymentService;

        public OrderingProcessManager(IOrderService orderService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
        }

        public void Handle(PaymentStatusChanged @event)
        {
            var payment = _paymentService.GetById(@event.PaymentId);

            if (payment.PaymentTarget.Type != PaymentTargetTypes.Order)
            {
                return;
            }
            if (@event.NewStatus != PaymentStatus.Success)
            {
                return;
            }

            var orderId = Convert.ToInt32(payment.PaymentTarget.Id);
            var order = _orderService.GetById(orderId);
            _orderService.AcceptPayment(order, payment);
        }
    }
}
