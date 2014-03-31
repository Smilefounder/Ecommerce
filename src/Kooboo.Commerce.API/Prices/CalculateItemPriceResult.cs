using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Prices
{
    public class CalculateItemPriceResult
    {
        public int Id { get; set; }

        public PriceWithDiscount Subtotal { get; set; }
    }
}
