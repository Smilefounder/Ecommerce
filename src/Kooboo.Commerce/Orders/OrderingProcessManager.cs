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
        public void Handle(PaymentStatusChanged @event, EventContext context)
        {
            var payment = new PaymentService(context.Instance).Find(@event.PaymentId);

            if (@event.NewStatus != PaymentStatus.Success)
            {
                return;
            }

            var orderId = @payment.OrderId;
            var orderService = new OrderService(context.Instance);
            var order = orderService.Find(orderId);
            orderService.AcceptPayment(order, payment);
        }
    }
}
