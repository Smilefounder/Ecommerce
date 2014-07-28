using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public class ApiContext
    {
        public string Instance { get; set; }

        public CultureInfo Culture { get; set; }

        public string Currency { get; set; }

        public string CustomerAccountId { get; set; }

        public ApiContext() { }

        public ApiContext(string instance, CultureInfo culture, string currency, string customerAccountId)
        {
            Instance = instance;
            Culture = culture;
            Currency = currency;
            CustomerAccountId = customerAccountId;
        }
    }
}
