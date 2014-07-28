using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local
{
    public class LocalApiContext : ApiContext
    {
        public ICommerceDatabase Database { get; set; }

        public IServiceFactory Services { get; set; }

        public LocalApiContext(ApiContext context, ICommerceDatabase database, IServiceFactory serviceFactory)
            : base(context.Instance, context.Culture, context.Currency, context.CustomerAccountId)
        {
            Database = database;
            Services = serviceFactory;
        }
    }
}
