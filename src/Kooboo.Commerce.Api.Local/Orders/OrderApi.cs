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
            var service = new Kooboo.Commerce.Carts.ShoppingCartService(_context.Instance);
            var cart = service.Find(cartId);

            return _context.Database.Transactional(() =>
            {
                var orderService = new Kooboo.Commerce.Orders.OrderService(_context.Instance);
                var order = orderService.CreateFromCart(cart, new Kooboo.Commerce.Carts.ShoppingContext
                {
                    Culture = context.Culture,
                    CustomerId = context.CustomerId
                });

                return order.Id;
            });
        }

        public PaymentResult Pay(PaymentRequest request)
        {
            var paymentMethod = _context.Database.Repository<Core.PaymentMethod>().Find(request.PaymentMethodId);
            var orderService = new Kooboo.Commerce.Orders.OrderService(_context.Instance);

            var processResult = orderService.ProcessPayment(new Commerce.Orders.PaymentRequest
            {
                OrderId = request.OrderId,
                Description = request.Description,
                Amount = request.Amount,
                CurrencyCode = request.CurrencyCode,
                ReturnUrl = request.ReturnUrl,
                PaymentMethod = paymentMethod,
                Parameters = request.Parameters
            });

            return new PaymentResult
            {
                Message = processResult.Message,
                PaymentId = processResult.PaymentId,
                PaymentStatus = (Kooboo.Commerce.Api.Payments.PaymentStatus)(int)processResult.PaymentStatus,
                RedirectUrl = processResult.RedirectUrl
            };
        }
    }
}
