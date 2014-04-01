using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Prices;
using Kooboo.Commerce.Orders.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommerceApi = Kooboo.Commerce.API;

namespace Kooboo.Commerce.API.LocalProvider.Prices
{
    [Dependency(typeof(IPriceAPI))]
    public class LocalPriceAPI : IPriceAPI
    {
        private Kooboo.Commerce.Orders.Services.IOrderService _orderService;
        private Kooboo.Commerce.Payments.Services.IPaymentMethodService _paymentMethodService;
        private Kooboo.Commerce.Customers.Services.ICustomerService _customerService;
        private Kooboo.Commerce.Products.Services.IProductService _productService;

        public LocalPriceAPI(
            Kooboo.Commerce.Orders.Services.IOrderService orderService,
            Kooboo.Commerce.Payments.Services.IPaymentMethodService paymentMethodService,
            Kooboo.Commerce.Customers.Services.ICustomerService customerService,
            Kooboo.Commerce.Products.Services.IProductService productService)
        {
            _orderService = orderService;
            _paymentMethodService = paymentMethodService;
            _customerService = customerService;
            _productService = productService;
        }

        public CalculatePriceResult Calculate(CalculatePriceRequest request)
        {
            var ctx = new PricingContext
            {
                CouponCode = request.CouponCode
            };

            if (request.CustomerId != null)
            {
                ctx.Customer = _customerService.GetById(request.CustomerId.Value);
            }
            if (request.PaymentMethodId != null)
            {
                ctx.PaymentMethod = _paymentMethodService.GetById(request.PaymentMethodId.Value);
            }

            foreach (var item in request.Items)
            {
                var price = _productService.GetProductPriceById(item.ProductPriceId);
                ctx.Items.Add(new Commerce.Orders.Pricing.PricingItem(item.Id, price, item.Quantity));
            }

            new PricingPipeline().Execute(ctx);

            return GetResult(ctx);
        }

        public CalculatePriceResult CalculateOrderPrice(CalculateOrderPriceRequest request)
        {
            var order = _orderService.GetById(request.OrderId);
            var ctx = PricingContext.CreateFrom(order);
            if (request.PaymentMethodId != null)
            {
                ctx.PaymentMethod = _paymentMethodService.GetById(request.PaymentMethodId.Value);
            }

            new PricingPipeline().Execute(ctx);

            return GetResult(ctx);
        }

        private CalculatePriceResult GetResult(PricingContext ctx)
        {
            var result = new CalculatePriceResult();

            foreach (var item in ctx.Items)
            {
                result.Items.Add(new CalculateItemPriceResult
                {
                    Id = item.Id,
                    Subtotal = item.Subtotal.ToDto()
                });
            }

            result.ShippingCost = ctx.ShippingCost.ToDto();
            result.PaymentMethodCost = ctx.PaymentMethodCost.ToDto();
            result.Tax = ctx.Tax.ToDto();
            result.Subtotal = ctx.Subtotal.ToDto();
            result.Total = ctx.Total;

            return result;
        }
    }
}
