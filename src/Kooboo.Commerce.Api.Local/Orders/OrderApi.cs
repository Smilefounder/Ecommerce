using System;
using System.Linq;
using Kooboo.Commerce.Api.Orders;
using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.Api.Payments;
using Core = Kooboo.Commerce.Payments;

namespace Kooboo.Commerce.Api.Local.Orders
{
    public class OrderApi : IOrderApi
    {
        private LocalApiContext _context;
        private Core.IPaymentProcessorProvider _paymentProcessorProvider;

        public OrderApi(LocalApiContext context, Core.IPaymentProcessorProvider paymentProcessorProvider)
        {
            _context = context;
            _paymentProcessorProvider = paymentProcessorProvider;
        }

        public Query<Order> Query()
        {
            return new Query<Order>(new OrderQueryExecutor(_context));
        }

        public int CreateFromCart(int cartId, ShoppingContext context)
        {
            var service = new Kooboo.Commerce.Carts.ShoppingCartService(_context.Database);
            var cart = service.GetById(cartId);

            return _context.Database.Transactional(() =>
            {
                var orderService = new Kooboo.Commerce.Orders.OrderService(_context.Database);
                var order = orderService.CreateFromCart(cart, new Kooboo.Commerce.Carts.ShoppingContext
                {
                    Culture = context.Culture,
                    Currency = context.Currency,
                    CustomerId = context.CustomerId
                });

                return order.Id;
            });
        }

        public PaymentResult Pay(PaymentRequest request)
        {
            var paymentMethod = _context.Database.Repository<Core.PaymentMethod>().Find(request.PaymentMethodId);
            var payment = new Kooboo.Commerce.Payments.Payment(request.OrderId, request.Amount, paymentMethod, request.Description);

            var paymentService = new Core.PaymentService(_context.Database);

            paymentService.Create(payment);

            // TODO: Consider move ProcessPayment to PaymentService
            var processor = _paymentProcessorProvider.FindByName(paymentMethod.ProcessorName);
            object config = null;

            if (processor.ConfigType != null)
            {
                config = paymentMethod.LoadProcessorConfig(processor.ConfigType);
            }

            var processResult = processor.Process(new Kooboo.Commerce.Payments.PaymentProcessingContext(payment, config)
            {
                CurrencyCode = request.CurrencyCode,
                ReturnUrl = request.ReturnUrl,
                Parameters = request.Parameters
            });

            paymentService.AcceptProcessResult(payment, processResult);

            return new PaymentResult
            {
                Message = processResult.Message,
                PaymentId = payment.Id,
                PaymentStatus = (Kooboo.Commerce.Api.Payments.PaymentStatus)(int)processResult.PaymentStatus,
                RedirectUrl = processResult.RedirectUrl
            };
        }
    }
}
