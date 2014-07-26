using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public class ApiContext
    {
        public string Instance { get; private set; }

        public CultureInfo Culture { get; private set; }

        public string Currency { get; private set; }

        public ApiContext(string instance, CultureInfo culture, string currency)
        {
            Instance = instance;
            Culture = culture;
            Currency = currency;
        }
    }
}
