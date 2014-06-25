using Kooboo.Commerce.API.Pricing;
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
        public CalculatePriceResult CartPrice(int cartId)
        {
            return Commerce().Prices.CartPrice(cartId);
        }
    }
}
