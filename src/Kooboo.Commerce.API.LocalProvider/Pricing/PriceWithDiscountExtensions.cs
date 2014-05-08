using Kooboo.Commerce.API.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Pricing
{
    public static class PriceWithDiscountExtensions
    {
        public static PriceWithDiscount ToDto(this Kooboo.Commerce.Orders.Pricing.PriceWithDiscount data)
        {
            return new PriceWithDiscount
            {
                OriginalValue = data.OriginalValue,
                Discount = data.Discount
            };
        }
    }
}
