using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Prices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Prices
{
    [Dependency(typeof(IPriceAPI))]
    public class RestPriceAPI : RestApiBase, IPriceAPI
    {
        protected override string ApiControllerPath
        {
            get { return "Price"; }
        }

        public CalculatePriceResult Calculate(CalculatePriceRequest request)
        {
            return Post<CalculatePriceResult>("Calculate", request);
        }

        public CalculatePriceResult CalculateOrderPrice(CalculateOrderPriceRequest request)
        {
            return Post<CalculatePriceResult>("CalculateOrderPrice", request);
        }
    }
}
