using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.Api.Local.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = Kooboo.Commerce.Carts;

namespace Kooboo.Commerce.Api.Local.Carts.Mapping
{
    public class ShoppingCartMapper : DefaultObjectMapper
    {
        public override object Map(object source, object target, Type sourceType, Type targetType, string prefix, MappingContext context)
        {
            var model = base.Map(source, target, sourceType, targetType, prefix, context) as ShoppingCart;
            var cart = source as Core.ShoppingCart;

            // Update cart prices
            var priceContext = new Core.ShoppingCartService(context.ApiContext.Instance).CalculatePrice(cart, null);
            if (model.Items != null && model.Items.Count > 0)
            {
                foreach (var priceItem in priceContext.Items)
                {
                    var cartItem = model.Items.FirstOrDefault(it => it.Id == priceItem.ItemId);
                    cartItem.Subtotal = priceItem.Subtotal;
                    cartItem.Discount = priceItem.Discount;
                    cartItem.Total = priceItem.Subtotal - priceItem.Discount;
                }
            }

            model.ShippingCost = priceContext.ShippingCost;
            model.PaymentMethodCost = priceContext.PaymentMethodCost;
            model.Tax = priceContext.Tax;

            model.Subtotal = priceContext.Subtotal;
            model.TotalDiscount = priceContext.TotalDiscount;
            model.Total = priceContext.Total;

            return model;
        }
    }
}
