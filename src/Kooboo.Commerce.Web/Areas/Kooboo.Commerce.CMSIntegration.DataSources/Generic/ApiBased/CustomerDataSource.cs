using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    public class CustomerDataSource : ApiBasedDataSource
    {
        public CustomerDataSource()
            : base("Customers", typeof(ICustomerQuery), typeof(Customer))
        {
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Customers;
        }
    }
}