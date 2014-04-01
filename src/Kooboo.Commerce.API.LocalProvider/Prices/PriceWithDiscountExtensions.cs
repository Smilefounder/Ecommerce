using Kooboo.Commerce.API.Prices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Prices
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
