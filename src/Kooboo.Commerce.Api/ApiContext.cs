using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public class ApiContext
    {
        public string InstanceName { get; set; }

        public CultureInfo Culture { get; set; }

        public string Currency { get; set; }

        public string CustomerEmail { get; set; }

        public ApiContext() { }

        public ApiContext(string instance, CultureInfo culture, string currency)
        {
            InstanceName = instance;
            Culture = culture;
            Currency = currency;
        }
    }
}
