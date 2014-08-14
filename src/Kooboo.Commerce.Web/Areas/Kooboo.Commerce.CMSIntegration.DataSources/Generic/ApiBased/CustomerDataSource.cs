using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(CustomerDataSource))]
    public class CustomerDataSource : ApiBasedDataSource<Customer>
    {
        public override string Name
        {
            get { return "Customers"; }
        }

        protected override Api.Query<Customer> Query(Api.ICommerceApi api)
        {
            return api.Customers.Query();
        }
    }
}