using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Customers.Services;

namespace Kooboo.Commerce.API.LocalAPI
{
    [Dependency(typeof(ICustomerAPI), ComponentLifeStyle.Transient, Key = "LocalAPI")]
    public class CustomerAPI : ICustomerAPI
    {
        private ICustomerService _customerService;

        public CustomerAPI(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public Customer GetCustomerById(int customerId)
        {
            return _customerService.GetById(customerId, true);
        }

        public Customer GetCustomerByAccount(string accountId)
        {
            return _customerService.GetByAccountId(accountId, true);
        }
    }
}
