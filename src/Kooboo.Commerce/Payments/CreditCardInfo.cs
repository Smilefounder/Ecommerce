using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class CreditCardInfo
    {
        public string HolderName { get; set; }

        public string CardNumber { get; set; }

        public string Cvv2 { get; set; }

        /// <summary>
        /// Two character expired year.
        /// </summary>
        public int ExpiredYear { get; set; }

        /// <summary>
        /// Two character expired month.
        /// </summary>
        public int ExpiredMonth { get; set; }
    }
}
