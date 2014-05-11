using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Pricing
{
    public interface IPriceAPI
    {
        CalculatePriceResult OrderPrice(CalculateOrderPriceRequest request);

        CalculatePriceResult CartPrice(int cartId);
    }
}
