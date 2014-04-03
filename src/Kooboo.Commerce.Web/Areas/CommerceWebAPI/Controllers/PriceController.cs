using Kooboo.Commerce.API.Prices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
{
    public class PriceController : CommerceAPIControllerBase
    {
        [HttpPost]
        public CalculatePriceResult Calculate(CalculatePriceRequest request)
        {
            return Commerce().Prices.Calculate(request);
        }

        [HttpPost]
        public CalculatePriceResult CalculateOrderPrice(CalculateOrderPriceRequest request)
        {
            return Commerce().Prices.CalculateOrderPrice(request);
        }
    }
}
