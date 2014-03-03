using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class BankAccountInfo
    {
        public string HolderName { get; set; }

        /// <summary>
        /// The BIC code for the customer bank account.
        /// </summary>
        public string BIC { get; set; }

        /// <summary>
        /// The IBAN for the customer bank account.
        /// </summary>
        public string IBAN { get; set; }
    }
}
