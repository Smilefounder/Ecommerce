using Kooboo.Commerce.API.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.API.HAL;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
{
    public class PriceController : CommerceAPIControllerBase
    {
        [HttpPost]
        [Resource("calculate_product_price")]
        public CalculatePriceResult Calculate(CalculatePriceRequest request)
        {
            return Commerce().Prices.Calculate(request);
        }

        [HttpPost]
        [Resource("calculate_order_price")]
        public CalculatePriceResult CalculateOrderPrice(CalculateOrderPriceRequest request)
        {
            return Commerce().Prices.CalculateOrderPrice(request);
        }
    }
}
