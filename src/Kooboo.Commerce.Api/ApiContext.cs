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

        public CustomerIdentity Customer { get; set; }

        public ApiContext()
        {
            Customer = CustomerIdentity.Guest();
        }

        public ApiContext(string instance, CultureInfo culture)
            : this()
        {
            InstanceName = instance;
            Culture = culture;
        }
    }
}
