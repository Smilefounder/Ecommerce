using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.RetailPriceDiscount
{
    public class RetailPriceDiscountActivityConfig : ActivityParameters
    {
        public DiscountMode DiscountMode
        {
            get
            {
                return GetValue<DiscountMode>("DiscountMode");
            }
            set
            {
                SetValue("DiscountMode", value);
            }
        }

        public decimal DiscountAmount
        {
            get
            {
                return GetValue<decimal>("DiscountAmount");
            }
            set
            {
                SetValue("DiscountAmount", value);
            }
        }

        public int DiscountPercentOff
        {
            get
            {
                return GetValue<int>("DiscountPercentOff");
            }
            set
            {
                SetValue("DiscountPercentOff", value);
            }
        }

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