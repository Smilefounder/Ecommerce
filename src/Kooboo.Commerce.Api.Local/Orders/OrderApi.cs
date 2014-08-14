using System;
using System.Linq;
using Kooboo.Commerce.Api.Orders;
using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.Api.Payments;
using Kooboo.Commerce.Payments;

namespace Kooboo.Commerce.Api.Local.Orders
{
    public class OrderApi : IOrderApi
    {
        private LocalApiContext _context;
        private IPaymentProcessorProvider _paymentProcessorProvider;

        public OrderApi(LocalApiContext context, IPaymentProcessorProvider paymentProcessorProvider)
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
            var cart = _context.Services.Carts.GetById(cartId);

            return _context.Database.WithTransaction(() =>
            {
                var order = _context.Services.Orders.CreateFromCart(cart, new Kooboo.Commerce.Carts.ShoppingContext
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
            var paymentMethod = _context.Services.PaymentMethods.GetById(request.PaymentMethodId);
            var payment = new Kooboo.Commerce.Payments.Payment(request.OrderId, request.Amount, paymentMethod, request.Description);

            _context.Services.Payments.Create(payment);

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

            _context.Services.Payments.AcceptProcessResult(payment, processResult);

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
