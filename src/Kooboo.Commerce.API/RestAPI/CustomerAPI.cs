using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Customers.Services;

namespace Kooboo.Commerce.API.RestAPI
{
    [Dependency(typeof(ICustomerAPI), ComponentLifeStyle.Transient, Key = "RestAPI")]
    public class CustomerAPI : RestApiBase, ICustomerAPI
    {
        public Customer GetCustomerById(int customerId)
        {
            return Get<Customer>(customerId.ToString());
        }

        public Customer GetCustomerByAccount(string accountId)
        {
            QueryParameters.Add("id", accountId);
            return Get<Customer>("GetByAccount");
        }

        protected override string ApiControllerPath
        {
            get { return "Customer"; }
        }
    }
}
