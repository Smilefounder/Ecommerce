using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Customers;

namespace Kooboo.Commerce.API
{
    public interface ICustomerAPI
    {
        Customer GetCustomerById(int customerId);

        Customer GetCustomerByAccount(string accountId);
    }
}
