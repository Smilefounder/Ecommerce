using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local
{
    public class LocalApiContext : ApiContext
    {
        public ICommerceDatabase Database { get; set; }

        public LocalApiContext(ApiContext context, ICommerceDatabase database)
        {
            Instance = context.Instance;
            Culture = context.Culture;
            Currency = context.Currency;
            CustomerEmail = context.CustomerEmail;
            Database = database;
        }

        public LocalApiContext(string instance, CultureInfo culture, string currency, ICommerceDatabase database)
            : base(instance, culture, currency)
        {
            Database = database;
        }
    }
}
