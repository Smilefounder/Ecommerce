using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Pricing;
using Kooboo.Commerce.Orders.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommerceApi = Kooboo.Commerce.API;

namespace Kooboo.Commerce.API.LocalProvider.Pricing
{
    [Dependency(typeof(IPriceAPI))]
    public class LocalPriceAPI : IPriceAPI
    {
        private Kooboo.Commerce.Orders.Services.IOrderService _orderService;
        private Kooboo.Commerce.ShoppingCarts.Services.IShoppingCartService _cartService;
        private Kooboo.Commerce.Payments.Services.IPaymentMethodService _paymentMethodService;
        private Kooboo.Commerce.Customers.Services.ICustomerService _customerService;
        private Kooboo.Commerce.Products.Services.IProductService _productService;

        public LocalPriceAPI(
            Kooboo.Commerce.Orders.Services.IOrderService orderService,
            Kooboo.Commerce.ShoppingCarts.Services.IShoppingCartService cartService,
            Kooboo.Commerce.Payments.Services.IPaymentMethodService paymentMethodService,
            Kooboo.Commerce.Customers.Services.ICustomerService customerService,
            Kooboo.Commerce.Products.Services.IProductService productService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _paymentMethodService = paymentMethodService;
            _customerService = customerService;
            _productService = productService;
        }

        public CalculatePriceResult CartPrice(int cartId)
        {
            var cart = _cartService.GetById(cartId);
            var context = _cartService.CalculatePrice(cart, null);
            return GetResult(context);
        }

        private CalculatePriceResult GetResult(PricingContext ctx)
        {
            var result = new CalculatePriceResult();

            foreach (var item in ctx.Items)
            {
                result.Items.Add(new CalculateItemPriceResult
                {
                    Id = item.ItemId,
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
