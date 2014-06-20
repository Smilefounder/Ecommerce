using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.RetailPriceDiscount
{
    public class RetailPriceDiscountActivityConfig
    {
        public DiscountMode DiscountMode { get; set; }

        public decimal DiscountAmount { get; set; }

        public int DiscountPercentOff { get; set; }

        public decimal ApplyDiscount(decimal oldPrice)
        {
            var newPrice = oldPrice;

            if (DiscountMode == DiscountMode.ByAmount)
            {
                newPrice -= DiscountAmount;
            }
            else
            {
                newPrice = oldPrice * (1 - (decimal)DiscountPercentOff / 100);
            }

            newPrice = Math.Round(newPrice, 2);

            if (newPrice < 0)
            {
                newPrice = 0;
            }

            return newPrice;
        }
    }
}