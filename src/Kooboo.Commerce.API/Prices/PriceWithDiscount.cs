using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Prices
{
    public class PriceWithDiscount
    {
        public decimal OriginalValue { get; set; }

        public decimal Discount { get; set; }

        public decimal FinalValue
        {
            get
            {
                return OriginalValue - Discount;
            }
        }
    }
}
