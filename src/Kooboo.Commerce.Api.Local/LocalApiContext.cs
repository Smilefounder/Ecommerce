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
        private CommerceInstance _instance;

        public CommerceInstance Instance
        {
            get
            {
                return _instance;
            }
        }

        public ICommerceDatabase Database
        {
            get
            {
                return _instance.Database;
            }
        }

        public LocalApiContext(ApiContext context, CommerceInstance instance)
        {
            InstanceName = context.InstanceName;
            Culture = context.Culture;
            Currency = context.Currency;
            CustomerEmail = context.CustomerEmail;
            _instance = instance;
        }

        public LocalApiContext(CultureInfo culture, string currency, CommerceInstance instance)
            : base(instance.Name, culture, currency)
        {
            _instance = instance;
        }
    }
}
