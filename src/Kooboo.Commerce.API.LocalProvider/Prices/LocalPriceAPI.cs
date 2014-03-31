using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Prices;
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
        private Kooboo.Commerce.Orders.IPriceCalculator _calculator;
        private Kooboo.Commerce.Orders.Services.IOrderService _orderService;
        private Kooboo.Commerce.Payments.Services.IPaymentMethodService _paymentMethodService;
        private Kooboo.Commerce.Customers.Services.ICustomerService _customerService;
        private Kooboo.Commerce.Products.Services.IProductService _productService;

        public LocalPriceAPI(
            Kooboo.Commerce.Orders.IPriceCalculator calculator,
            Kooboo.Commerce.Orders.Services.IOrderService orderService,
            Kooboo.Commerce.Payments.Services.IPaymentMethodService paymentMethodService,
            Kooboo.Commerce.Customers.Services.ICustomerService customerService,
            Kooboo.Commerce.Products.Services.IProductService productService)
        {
            _calculator = calculator;
            _orderService = orderService;
            _paymentMethodService = paymentMethodService;
            _customerService = customerService;
            _productService = productService;
        }

        public CalculatePriceResult Calculate(CalculatePriceRequest request)
        {
            var ctx = new Kooboo.Commerce.Orders.PriceCalculationContext
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
                ctx.Items.Add(new Commerce.Orders.PricingItem(item.Id, price, item.Quantity));
            }

            _calculator.Calculate(ctx);

            return GetResult(ctx);
        }

        public CalculatePriceResult CalculateOrderPrice(CalculateOrderPriceRequest request)
        {
            var order = _orderService.GetById(request.OrderId);
            var ctx = Kooboo.Commerce.Orders.PriceCalculationContext.CreateFrom(order);
            if (request.PaymentMethodId != null)
            {
                ctx.PaymentMethod = _paymentMethodService.GetById(request.PaymentMethodId.Value);
            }

            _calculator.Calculate(ctx);

            return GetResult(ctx);
        }

        private CalculatePriceResult GetResult(Kooboo.Commerce.Orders.PriceCalculationContext ctx)
        {
            var result = new CalculatePriceResult();

            foreach (var item in ctx.Items)
            {
                result.Items.Add(new PricingItem
                {
                    Id = item.Id,
                    ProductPriceId = item.ProductPrice.Id,
                    Quantity = item.Quantity,
                    Subtotal = item.Subtotal,
                    Discount = item.Discount
                });
            }

            result.DiscountExItemDiscounts = ctx.DiscountExItemDiscounts;
            result.ShippingCost = ctx.ShippingCost;
            result.ShippingDiscount = ctx.ShippingDiscount;
            result.PaymentMethodCost = ctx.PaymentMethodCost;
            result.PaymentMethodDiscount = ctx.PaymentMethodDiscount;
            result.Tax = ctx.Tax;
            result.Subtotal = ctx.Subtotal;
            result.Total = ctx.Total;

            return result;
        }
    }
}
