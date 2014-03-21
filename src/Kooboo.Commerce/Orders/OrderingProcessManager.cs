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
        : IHandles<PaymentStatusChanged>
    {
        private IOrderService _orderService;
        private IPaymentService _paymentService;

        public OrderingProcessManager(IOrderService orderService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
        }

        public void Handle(PaymentStatusChanged @event, EventDispatchingContext context)
        {
            if (@event.Payment.PaymentTargetType != PaymentTargetTypes.Order)
            {
                return;
            }
            if (@event.NewStatus != PaymentStatus.Success)
            {
                return;
            }

            var orderId = Convert.ToInt32(@event.Payment.PaymentTargetId);
            var order = _orderService.GetById(orderId);

            var otherSuccessPayments = _paymentService.Query()
                                                      .ByTarget(PaymentTargetTypes.Order, orderId.ToString())
                                                      .WhereSucceeded()
                                                      .Where(x => x.Id != @event.Payment.Id)
                                                      .ToList();

            var paidTotal = @event.Payment.Amount + otherSuccessPayments.Sum(x => x.Amount);
            if (paidTotal >= order.Total)
            {
                order.ChangeStatus(OrderStatus.Paid);
            }
        }
    }
}
