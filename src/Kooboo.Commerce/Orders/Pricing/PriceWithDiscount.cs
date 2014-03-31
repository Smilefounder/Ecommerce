using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing
{
    /// <summary>
    /// Represents a price value and the corresponding discount.
    /// </summary>
    public class PriceWithDiscount
    {
        public decimal OriginalValue { get; private set; }

        public decimal Discount { get; private set; }

        public decimal FinalValue
        {
            get
            {
                return OriginalValue - Discount;
            }
        }

        public PriceWithDiscount() { }

        public PriceWithDiscount(decimal originalValue)
        {
            OriginalValue = originalValue;
        }

        public void AddDiscount(decimal amount)
        {
            var newDiscount = Discount + amount;
            if (newDiscount > OriginalValue)
            {
                newDiscount = OriginalValue;
            }

            Discount = newDiscount;
        }

        public void SetOriginalValue(decimal price)
        {
            OriginalValue = price;
            if (Discount > OriginalValue)
            {
                Discount = OriginalValue;
            }
        }
    }
}
