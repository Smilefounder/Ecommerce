using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Events.Payments;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders
{
    public class OrderingProcessManager
        : IHandle<PaymentStatusChanged>
    {
        private IOrderService _orderService;

        public OrderingProcessManager(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public void Handle(PaymentStatusChanged @event)
        {
            if (@event.Payment.PaymentTarget.Type != PaymentTargetTypes.Order)
            {
                return;
            }
            if (@event.NewStatus != PaymentStatus.Success)
            {
                return;
            }

            var orderId = Convert.ToInt32(@event.Payment.PaymentTarget.Id);
            var order = _orderService.GetById(orderId);
            order.AcceptPayment(@event.Payment);
        }
    }
}
